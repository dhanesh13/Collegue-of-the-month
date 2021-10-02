import { Component, OnInit, ViewChild, ViewEncapsulation, ElementRef, Output, EventEmitter, Inject, ChangeDetectorRef, AfterViewInit} from '@angular/core';

import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { Nominees } from 'app/model/nominees-model';
import { ApiService } from 'app/api.service';
import { DataService } from 'app/data.service';
import { RationaleComponent } from './rationale/rationale.component';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatSort} from '@angular/material/sort';
import { ViewportScroller } from '@angular/common';
import { EventVotingSession } from 'app/model/event-voting-session';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { VotingSession } from 'app/model/voting-session-model';
import { EventModel } from 'app/model/event';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DashboardComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['employeeId', 'query.EmployeeFirstName', 'query.EmployeeDivision',
                                 'query.EmployeeDepartment', 'query.EmployeeFirstNameManager','rationale'];

  dataSource;
  nominationPeriod;

  // top nominees by company
  displayedColumnsCompany: string[] = ['employeeId', 'query.employeeFirstName',
                                        'query.employeeDivision', 'query.employeeDepartment', 'query.employeeFirstNameManager','nominationsCount'];
  dataSourceCompany;

  //top 5 nominees by company
  displayedColumnsCompanyTopFive: string[] = ['query.employeeFirstName', 'query.employeeDepartment', 'nominationsCount'];
  dataSourceCompanyTop5;

  //active voting sessions
  displayedColumnsVotingSessions: string[] = ['eventDescription', 'period', 'openingDate', 'closingDate'];
  dataSourceVotingSession;

  //top 5 non-shortlisted nominees
  displayedColumnTop5NonShortlisted: string[] = ['query.EmployeeFirstName', 'query.EmployeeDepartment', 'nominationsCount'];
  dataSourceTop5NonShortlisted;

  //display opened events
  displayedColumnsOpenedEvents: string[] = ['eventDescription'];
  dataSourceOpenedEvents;

  period;


  minOpeningDate: Date;
  maxOpeningDate: Date;
  minClosingDate: Date;
  maxClosingDate: Date;
 
  @ViewChild('TableShortlistPaginator', {static: true}) tableShortlistPaginator: MatPaginator;
  @ViewChild('TableShortlistSort', {static: true}) tableShortlistSort: MatSort;

  @ViewChild('TableCompanyPaginator', {static: true}) tableCompanyPaginator: MatPaginator;
  @ViewChild('TableCompanySort', {static: true}) tableCompanySort: MatSort;

  @ViewChild('TableTop5NomineesSort', {static: true}) tableTop5NomineesSort: MatSort;

  @ViewChild('TableTop5NonShortlistedSort', {static: true}) tableTop5NonShortlistedSort: MatSort;

  @ViewChild('TableVotingSessionSort', {static: true}) tableVotingSessionSort: MatSort;

  @ViewChild('TableOpenedEventsSort', {static: true}) tableOpenedEventsSort: MatSort;

  

  @ViewChild("aboutus", { static: false }) aboutus;

  @Output() Navigate = new EventEmitter();

  public events: EventModel[];

  public topNominees: Nominees[];
  public topFiveNominees: Nominees[];
  public eventVotingSessions: EventVotingSession[];
  eventId;

  formVotingSession: FormGroup;
  votingSessionForm: VotingSession = new VotingSession();
  

  title = 'appBootstrap';
  
  closeResult: string;

  // first table
  searchKey: string;
  isTableHasData = true;

  // second table
  searchKey1: string;
  isTableHasData1 = true;

  numberOfActiveSessions;
  numberOfOpenedEvents;

  constructor(private apiService:ApiService,private dataService:DataService,private modalService: NgbModal,
    private formBuilder: FormBuilder, private viewportScroller: ViewportScroller, private changeDetector: ChangeDetectorRef) {

    const currentYear = new Date().getFullYear();
    this.minOpeningDate = new Date();
    this.maxOpeningDate = new Date(currentYear + 1, 11, 31);
    this.minClosingDate = new Date();
    this.maxClosingDate = new Date(currentYear + 1, 11, 31);

  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return  `with: ${reason}`;
    }
  }
  ngAfterViewInit() {
    this.retrieveTopNominees();
  }
  ngOnInit() {
    // this.retrieveTopNominees();
    this.sessionList();

    // get current nomination period
    this.apiService.getPeriod()
    .subscribe((data) => {

      this.nominationPeriod = data;
    });
  }

  navigate() {
    document.getElementById("content").scrollIntoView();
  }


  // Voting sessions currently active
  sessionList(): void {
    this.apiService.getVotingSessions()
      .subscribe(
        (data: EventVotingSession[]) => {
          this.eventVotingSessions = data;
          this.numberOfActiveSessions= data.length;
          this.dataSourceVotingSession = new MatTableDataSource(this.eventVotingSessions);
          this.dataSourceVotingSession.sortingDataAccessor = (item, property) => {
            if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
            return item[property];
         };
          this.dataSourceVotingSession.sort = this.tableVotingSessionSort;
        },
        error => {
          console.log(error);
        });
  }

  // first table
  onSearchClear()
  {
    this.searchKey = "";
    this.applyFilter();
  }

  applyFilter()
  {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
    if(this.dataSource.filteredData.length > 0){
      this.isTableHasData = true;
    } else {
      this.isTableHasData = false;
    }
  }

  // second table
  onSearchClear1()
  {
    this.searchKey1 = "";
    this.applyFilter1();
  }

  applyFilter1()
  {
    this.dataSourceCompany.filter = this.searchKey1.trim().toLowerCase();
    if(this.dataSourceCompany.filteredData.length > 0){
      this.isTableHasData1 = true;
    } else {
      this.isTableHasData1 = false;
    }
  }

  retrieveTopNominees(): void {
    this.apiService.getTopShortlistedNomineesDetails()
      .subscribe(
        (data: Nominees[] ) => {
          this.topNominees = data;
          this.dataSource = new MatTableDataSource(this.topNominees);      
          this.dataSource.sortingDataAccessor = (item, property) => {
            if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
            return item[property];
         };
          this.dataSource.paginator = this.tableShortlistPaginator;
          this.dataSource.sort = this.tableShortlistSort;
        
          // filtering based on specific columns
          this.dataSource.filterPredicate = function(data: Nominees, filterValue: string) {
            return data.query.EmployeeFirstName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0 
                || data.query.EmployeeLastName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0            
                || data.query.EmployeeDivision.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
                || data.query.EmployeeDepartment.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
                || data.query.EmployeeFirstNameManager.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
                || data.query.EmployeeLastNameManager.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0;
          };
      });

      // top nominees by company
    this.apiService.getTopNomineesDetails()
    .subscribe(
      (data: Nominees[] ) => {
        this.topNominees = data;
        this.dataSourceCompany = new MatTableDataSource(this.topNominees);
        this.dataSourceCompany.sortingDataAccessor = (item, property) => {
          if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
          return item[property];
       };
        this.dataSourceCompany.paginator = this.tableCompanyPaginator;
        this.dataSourceCompany.sort = this.tableCompanySort;
        
      // filtering based on specific columns
      this.dataSourceCompany.filterPredicate = function(data: Nominees, filterValue: string) {
        return data.query.employeeFirstName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0 
            || data.query.employeeLastName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0            
            || data.query.employeeDivision.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
            || data.query.employeeDepartment.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
            || data.query.employeeFirstNameManager.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
            || data.query.employeeLastNameManager.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0;
      };

      }); 

        // top 5 nominees by company
    this.apiService.getTopFiveNomineesDetails()
    .subscribe(
      (data: Nominees[] ) => {
        this.topFiveNominees = data;
        this.dataSourceCompanyTop5 = new MatTableDataSource(this.topFiveNominees);
        this.dataSourceCompanyTop5.sortingDataAccessor = (item, property) => {
          if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
          return item[property];
       };
        this.dataSourceCompanyTop5.sort = this.tableTop5NomineesSort;
        
      }); 

      //top 5 non-shortlisted nominees - rejected
        this.apiService.getTopFiveNonShortlistedNominees()
          .subscribe(
            (data: Nominees[]) => {
              
              this.topNominees = data;
              this.dataSourceTop5NonShortlisted = new MatTableDataSource(this.topNominees);
              this.dataSourceTop5NonShortlisted.sortingDataAccessor = (item, property) => {
                if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
                return item[property];
             };
              this.dataSourceTop5NonShortlisted.sort = this.tableTop5NonShortlistedSort;
          });
      
         //display opened events
         this.apiService.getVotingSessions()
         .subscribe(
           (data: EventVotingSession[]) => {
             this.eventVotingSessions = data;
             this.numberOfOpenedEvents = data.length;
             this.dataSourceOpenedEvents = new MatTableDataSource(this.eventVotingSessions);
             this.dataSourceOpenedEvents.sortingDataAccessor = (item, property) => {
              if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
              return item[property];
           };
            this.dataSourceOpenedEvents.sort = this.tableOpenedEventsSort;
           
           },
           error => {
             console.log(error);
           }); 
  }


  onClickViewRationale(nom:Nominees){

    this.modalService.open(RationaleComponent, { size: 'md', backdrop: 'static' ,ariaLabelledBy: 'modal-basic-title',windowClass : "myCustomModalClass"}).result.then((result) => {
           this.closeResult = `Closed with: ${result}`;
         }, (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        });

  this.dataService.setData(nom);

  }

}

