import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import { ApiService } from 'app/api.service';
import { Nominees } from 'app/model/nominees-model';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatSort} from '@angular/material/sort';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VotingSessionService } from 'app/guards/voting-session.service';
import { EventVotingSession } from 'app/model/event-voting-session';
import { AuthService } from 'app/guards/auth.service';
import { AdminBasketModalComponent } from './admin-basket-modal/admin-basket-modal.component';
import { DataService } from 'app/data.service';
import { RationaleComponent } from 'app/dashboard/rationale/rationale.component';

@Component({
  selector: 'app-admin-basket',
  templateUrl: './admin-basket.component.html',
  styleUrls: ['./admin-basket.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AdminBasketComponent implements OnInit {

  displayedColumns: string[] = ['employeeId', 'query.EmployeeFirstName', 'query.EmployeeDivision',
   'query.EmployeeDepartment', 'query.EmployeeFirstNameManager','actions'];

  dataSource;
  nominationPeriod;

  show = false;
 
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  public topNominees : Nominees[];
  closeResult: string;

  public textAreaForm: FormGroup;

  data:EventVotingSession;

  searchKey: string;

  isTableHasData = true;

    constructor(private apiService: ApiService,  private dataService: DataService,private modalService: NgbModal, private fb : FormBuilder,
    public authService: AuthService) {

    this.textAreaForm = fb.group({
      textArea: ["",[Validators.required, Validators.maxLength(225),Validators.minLength(25)]],
    });
   }


  ngOnInit(): void {
    this.retrieveTopNonShortlistedNominees();

     // get current nomination period
     this.apiService.getPeriod()
     .subscribe((data) => {
 
       this.nominationPeriod = data;
     });

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

  retrieveTopNonShortlistedNominees(): void {
    this.apiService.getTopNonShortlistedNomineesDetails()
      .subscribe(
        (data: Nominees[]) => {
          
          this.topNominees = data;
          this.dataSource = new MatTableDataSource(this.topNominees);
          this.dataSource.sortingDataAccessor = (item, property) => {
            if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
            return item[property];
         };
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;

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
  }
  addAdminRationale(nom:Nominees){
    //this.modalService.open(RationaleComponent);

    this.modalService.open(AdminBasketModalComponent, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
           this.closeResult = `Closed with: ${result}`;
         }, (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        });
this.dataService.setData(nom);

  }
  onClickViewRationale(nom:Nominees){
    //this.modalService.open(RationaleComponent);
  
    this.modalService.open(RationaleComponent, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
           this.closeResult = `Closed with: ${result}`;
         }, (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        });
  
  this.dataService.setData(nom);
  
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
 
}


