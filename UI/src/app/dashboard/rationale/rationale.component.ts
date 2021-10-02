import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ApiService } from 'app/api.service';
import { DataService } from 'app/data.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-rationale',
  templateUrl: './rationale.component.html',
  styleUrls: ['./rationale.component.css']
})
export class RationaleComponent implements OnInit {
  active = 1;
  recievedData: any;
  recievedData$:number;
  viewRationaleNomineeFirstName$:string;
  viewRationaleNomineeLastName$:string;
  managerRationale$:string;
  adminRationale$:string;


  nominations$:any;
  status = ['Select Status', 'Impact', 'BeASpark'];
  selected:any;
  filtered:any;

  constructor(private apiService:ApiService,private dataService:DataService,public activeModal: NgbActiveModal) { }

  ngOnInit(): void {

    this.dataService.data.subscribe(
      response => {
        this.recievedData = response;
      });
      this.recievedData$=this.recievedData.employeeId;
      this.viewRationaleNomineeFirstName$=this.recievedData.query.EmployeeFirstName;
      this.viewRationaleNomineeLastName$=this.recievedData.query.EmployeeLastName;
      this.nominations$=this.recievedData.nominations;
      this. managerRationale$=this.recievedData.nominations.ManagerRationale;
      this.adminRationale$=this.recievedData.nominations.AdminRationale;
  }

}
