import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { ApiService } from "app/api.service";
import { Nominees } from 'app/model/nominees-model';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatSort} from '@angular/material/sort';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RationaleComponent } from 'app/dashboard/rationale/rationale.component';
import { DataService } from 'app/data.service';
import { AuthService } from 'app/guards/auth.service';
import { ManagerRationaleModalComponent } from './manager-rationale-modal/manager-rationale-modal.component';
import { RoleService } from 'app/guards/role.service';
import { NotificationService } from 'app/notification.service';
import { ManagerRejectionModelComponent } from './manager-rejection-model/manager-rejection-model.component';
import { InspireTeam } from 'app/model/inspire-team';
import { RationaleService } from 'app/rationale.service';
import { RationaleInspireComponent } from './rationale/rationale-inspire/rationale-inspire.component';
import { RationaleSparkComponent } from './rationale/rationale-spark/rationale-spark.component';

@Component({
  selector: 'app-manager',
  templateUrl: './manager.component.html',
  styleUrls: ['./manager.component.css']
})

export class ManagerComponent implements OnInit {
 
  // first card
  displayedColumnsCard: string[] =['query.EmployeeFirstName' , 'query.EmployeeDepartment' ,'nominationsCount'];
  dataSourceCard;

  // second card
  displayedColumnsSecondCard: string[] =['query.EmployeeFirstName' , 'query.EmployeeDepartment' ,'nominationsCount'];
  dataSourceSecondCard;

  // first table
  displayedColumns: string[] = ['employeeId', 'query.EmployeeFirstName', 'query.EmployeeDivision', 'query.EmployeeDepartment', 'query.EmployeeFirstNameManager','nominationsCount', 'actions'];
  dataSource;

  // contenders - second table
  displayedColumnsShortlist: string[] = ['employeeId', 'query.EmployeeFirstName', 'query.EmployeeDivision', 'query.EmployeeDepartment', 'query.EmployeeFirstNameManager','nominationsCount', 'actions'];
  dataSourceShortlist;

  // top nominees by company - third table
  displayedColumnsCompany: string[] = ['employeeId', 'query.employeeFirstName', 'query.employeeDivision', 'query.employeeDepartment', 'query.employeeFirstNameManager','nominationsCount'];
  dataSourceCompany;
  
  // Inspire Team - fourth table
  inspireTeamColumns: string[] = ['teamName','impact', 'beASpark'];
  dataSourceTeam;
  
  show = false;

  @ViewChild('TableOverallNominationsPaginator', {static: true}) tableCompanyPaginator: MatPaginator;
  @ViewChild('TableOverallNominationsSort', {static: true}) tableCompanySort: MatSort;

  @ViewChild('TableContendersPaginator', {static: true}) tableContendersPaginator: MatPaginator;
  @ViewChild('TableContendersSort', {static: true}) tableContendersSort: MatSort;

  @ViewChild('TableTopNomineesPaginator', {static: true}) tableTopNomineesPaginator: MatPaginator;
  @ViewChild('TableTopNomineesSort', {static: true}) tableTopNomineesSort: MatSort;

  @ViewChild('TableNomineesInspirePaginator', {static: true}) tableNomineesInspirePaginator: MatPaginator;
  @ViewChild('TableNomineesInspireSort', {static: true}) tableNomineesInspireSort: MatSort;

  @ViewChild('TableTop5ShorlistedSort', {static: true}) tableTop5ShorlistedSort: MatSort;

  @ViewChild('TableTop5NonShorlistedSort', {static: true}) tableTop5NonShorlistedSort: MatSort;

  public topNominees: Nominees[];
  public topNomineesByManager: Nominees[];
  public topNomineesByManagerCard: Nominees[];
  public shotlistedNomineesByManager :Nominees[];
  public topNomineesInspire: InspireTeam[];
  
  public nominationPeriod:string;

  EmployeeID: Number;
  FullName: string ;
  FirstName: string;
  LastName: string;
  Division: string;
  Department: string;
  Manager: string;
  NoofVotes: Number;
  VotedBy: string;
  public textAreaForm: FormGroup;

  managerId: string;
  closeResult: string;
  managerID: number;
  results;

  // search for first table
  searchKey: string;
  isTableHasData = true;

  // search for second table
  searchKey1: string;
  isTableHasData1 = true;

  // search for third table
  searchKey2: string;
  isTableHasData2 = true;

  // search for fourth table
  searchKey3: string;
  isTableHasData3 = true;

  constructor(private modalService: NgbModal, private apiService: ApiService, private cdr: ChangeDetectorRef,
    private fb : FormBuilder,private dataService:DataService, public authService: AuthService, public roleService: RoleService,
    private notifyService : NotificationService ,private rationaleService: RationaleService) { 
    this.textAreaForm = fb.group({
      textArea: ["",[Validators.required, Validators.maxLength(225),Validators.minLength(25)]],
    });
  }

  open(content) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
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

  ngOnInit(): void {
    
    //get nomination period
    this.apiService.getPeriod().subscribe((data)=>{

      this.nominationPeriod=data;
    });

    this.retrieveTopNominees();
    this.getNomPeriod();

    // opening and closing date for Manager ShorlistPeriod
    this.authService.getAccessShortlistPeriod()
    .subscribe((data) => {
   
      if(data == true)
      {
        this.show = false;  // show
      }
     
      if(data == false)
      {
        this.show = true; // hide
      }
      
    });
  }

  // view all for first card
  navigate() {
    document.getElementById("content").scrollIntoView();
  }

  // view all for second card
  navigateSecondCard() {
    document.getElementById("contentSecondCard").scrollIntoView();
  }

  // search for first table
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

  // search for second table
  onSearchClear1()
  {
    this.searchKey1 = "";
    this.applyFilter1();
  }

  applyFilter1()
  {
    this.dataSourceShortlist.filter = this.searchKey1.trim().toLowerCase();
    if(this.dataSourceShortlist.filteredData.length > 0){
      this.isTableHasData1 = true;
    } else {
      this.isTableHasData1 = false;
    }
  }

  // search for third table
  onSearchClear2()
  {
    this.searchKey2 = "";
    this.applyFilter2();
  }

  applyFilter2()
  {
    this.dataSourceCompany.filter = this.searchKey2.trim().toLowerCase();
    if(this.dataSourceCompany.filteredData.length > 0){
      this.isTableHasData2 = true;
    } else {
      this.isTableHasData2 = false;
    }
  }

  // search for fourth table
  onSearchClear3()
  {
    this.searchKey3 = "";
    this.applyFilter3();
  }

  applyFilter3()
  {
    this.dataSourceTeam.filter = this.searchKey3.trim().toLowerCase();
    if(this.dataSourceTeam.filteredData.length > 0){
      this.isTableHasData3 = true;
    } else {
      this.isTableHasData3 = false;
    }
  }

  getNomPeriod() {
    var months = new Array("January", "February", "March", "April", "May", "June",
      "July", "August", "September", "October", "November", "December");

    var todayDate = new Date();
    var monthName = months[todayDate.getMonth()];
    var year = todayDate.getFullYear();
    this.nominationPeriod = monthName + " " + year;
    return this.nominationPeriod;
  }
 
  retrieveTopNominees(): void {

    // first card
    this.apiService.GetTopFiveShortListedNomineesByManager()
    .subscribe(
      (data: Nominees[] ) => {
        this.shotlistedNomineesByManager = data;
        this.dataSourceCard = new MatTableDataSource(this.shotlistedNomineesByManager);
        this.dataSourceCard.sortingDataAccessor = (item, property) => {
          if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
          return item[property];
       };
        this.dataSourceCard.sort = this.tableTop5ShorlistedSort;
      }); 

    // second card
    this.apiService.getTopFiveNomineesDetailsByManager()
    .subscribe(
      (data: Nominees[] ) => {
        this.topNomineesByManager = data;
        this.dataSourceSecondCard = new MatTableDataSource(this.topNomineesByManager);
        this.dataSourceSecondCard.sortingDataAccessor = (item, property) => {
          if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
          return item[property];
       };
        this.dataSourceSecondCard.sort = this.tableTop5NonShorlistedSort; 
      }); 
  
    // top nominees by manager - first table
    this.apiService.getTopNomineesDetailsByManager()
        .subscribe(
            (data: Nominees[] ) => {
              
              this.topNomineesByManager = data;
              this.dataSource = new MatTableDataSource(this.topNomineesByManager);
              this.cdr.detectChanges();
              this.dataSource.sortingDataAccessor = (item, property) => {
                if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
                return item[property];
             };
              this.dataSource.paginator = this.tableTopNomineesPaginator;
              this.dataSource.sort = this.tableTopNomineesSort;

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

    //shortlisted by manager - second table
    this.apiService.GetShortListedNomineesByManager()
    .subscribe(
      (data: Nominees[] ) => {
              
        this.shotlistedNomineesByManager = data;
        this.dataSourceShortlist = new MatTableDataSource(this.shotlistedNomineesByManager);
        this.dataSourceShortlist.sortingDataAccessor = (item, property) => {
          if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
          return item[property];
       };
        this.dataSourceShortlist.paginator = this.tableContendersPaginator;
        this.dataSourceShortlist.sort = this.tableContendersSort;

        // filtering based on specific columns
        this.dataSourceShortlist.filterPredicate = function(data: Nominees, filterValue: string) {
          return data.query.EmployeeFirstName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0 
              || data.query.EmployeeLastName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0            
              || data.query.EmployeeDivision.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
              || data.query.EmployeeDepartment.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
              || data.query.EmployeeFirstNameManager.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
              || data.query.EmployeeLastNameManager.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0;
        };
    });

    // top nominees by company - third table
    this.apiService.getTopNomineesDetails()
      .subscribe(
        (data: Nominees[] ) => {
          this.topNominees = data;
          // console.log(this.topNominees)
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
        
      // inspire team - fourth table
      this.apiService.getNomineesInspire()
      .subscribe(
        (data: InspireTeam[] ) => {
          this.topNomineesInspire = data;
          this.dataSourceTeam = new MatTableDataSource(this.topNomineesInspire);
          this.dataSourceTeam.sortingDataAccessor = (item, property) => {
            if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
            return item[property];
        };
          this.dataSourceTeam.paginator = this.tableNomineesInspirePaginator;
          this.dataSourceTeam.sort = this.tableNomineesInspireSort;

          // filtering based on specific columns
          this.dataSourceTeam.filterPredicate = function(data: InspireTeam, filterValue: string) {
            return data.teamName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0;
          };
        }); 
  }

 
addManagerRationale(nom:Nominees){
  //this.modalService.open(RationaleComponent);

  this.modalService.open(ManagerRationaleModalComponent, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
         this.closeResult = `Closed with: ${result}`;
       }, (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      });
this.dataService.setData(nom);

}
addRejectionRationale(nom:Nominees){
 

  this.modalService.open(ManagerRejectionModelComponent, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
         this.closeResult = `Closed with: ${result}`;
       }, (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      });
this.dataService.setData(nom);

}
managerShortlistDirectly(emp:number){
  this.apiService.managerShortlistNominees(1,emp).subscribe(res=>{
  })
  this.notifyService.showSuccess("You have successfully shortlisted ","");
  setTimeout(() => {
   window.location.reload();
 }, 3000);

}

onClickViewRationale(nom:Nominees)
{
  this.modalService.open(RationaleComponent, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
    this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
  });
  this.dataService.setData(nom);
}

// impact rationale for inspire team
onClickViewRationaleImpact(nom:InspireTeam)
{
  this.modalService.open(RationaleInspireComponent, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
    this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
  });
  this.rationaleService.setData(nom);
}

// spark rationale for inspire team
onClickViewRationaleSpark(nom:InspireTeam)
{
  this.modalService.open(RationaleSparkComponent, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
    this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
  });
  this.rationaleService.setData(nom);
}


}
