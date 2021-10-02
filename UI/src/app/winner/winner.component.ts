import { Component, OnInit, ViewChild, ViewEncapsulation, TemplateRef } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApiService } from 'app/api.service';
import { Winner } from 'app/model/winner-model';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import {MatSort} from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { lang } from 'moment';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { WinnerService } from 'app/winner.service';



@Component({
  selector: 'app-winner',
  templateUrl: './winner.component.html',
  styleUrls: ['./winner.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class WinnerComponent implements OnInit {

  closeResult: string;
  @ViewChild('content') content;

  displayedColumns: string[] = ['query.EmployeeFirstName', 'query.EmployeeDivision', 'query.EmployeeDepartment', 'managerVotesCount', 'delete', 'declare',  'discardWinner'];
  
  dataSource;
  nominationPeriod;
 
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  winner: Winner[];

  searchKey: string;

  isTableHasData = true;

  modalRef: BsModalRef;

  eFirstName: string;
  eLastName: string;

  scrap: string;
  x: Observable<any>;

  @ViewChild('template') template : TemplateRef<any>;
  @ViewChild('templateEdit') templateEdit : TemplateRef<any>;
  @ViewChild('templateConfirmWinner') templateConfirmWinner : TemplateRef<any>;

  constructor(private modalService: NgbModal,private apiService: ApiService, private mService: BsModalService,
    public winnerService: WinnerService) { }

  ngAfterViewInit() {
    //this.openModal();
  }
  openModal(){
    this.modalService.open(this.content, { centered: true });
  }

   ngOnInit() {

    this.refreshDepList();

    // get current nomination period
    this.apiService.getPeriod()
    .subscribe((data) => {

      this.nominationPeriod = data;
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

  refreshDepList(): void {
    this.apiService.getManagerResult()
      .subscribe(
        (data: Winner[] ) => {
          this.winner = data;
          this.dataSource = new MatTableDataSource(this.winner);
          this.dataSource.sortingDataAccessor = (item, property) => {
            if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
            return item[property];
         };
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;

          // filtering based on specific columns
          this.dataSource.filterPredicate = function(data: any, filterValue: string) {
            return data.query.EmployeeFirstName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0 
                || data.query.EmployeeLastName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
                || data.query.EmployeeDivision.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0
                || data.query.EmployeeDepartment.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0;
          };
          
        },
        error => {
          console.log(error);
        });
  }

  // delete modal
  delete(d) {
    this.eFirstName =d.query.EmployeeFirstName;
    this.eLastName =d.query.EmployeeLastName;
    this.modalRef = this.mService.show(this.template); 

    this.winnerService.setData(d); 
  }

  confirm(): void {
    this.apiService.adminDeleteWinner(this.winnerService.dataWinner).subscribe(res=>{
    }); 
    this.modalRef.hide();
    window.location.reload();
  }
 
  decline(): void {
    this.modalRef.hide();
  }
  
  // edit modal 
  Edit(d) {
    this.eFirstName =d.query.EmployeeFirstName;
    this.eLastName =d.query.EmployeeLastName;
    this.modalRef = this.mService.show(this.templateEdit); 

    this.winnerService.setData(d); 
  }

  confirmEdit(): void {
    this.apiService.adminWinnerNominee(this.winnerService.dataWinner).subscribe(res=>{
    });
    this.modalRef.hide();
    window.location.reload();
  }
 
  declineEdit(): void {
    this.modalRef.hide();
  }

  // confirm winner modal 
  confirmWinner() {
   
     this.modalRef = this.mService.show(this.templateConfirmWinner); 

  }

  confirmYes(): void {
     this.apiService.sendEmailForWinners().subscribe();
    this.modalRef.hide();
    setTimeout(() => {
      window.location.reload();
    }, 5000);
    //window.location.reload();
  }
 
  declineNo(): void {
    this.modalRef.hide();
  }

}
