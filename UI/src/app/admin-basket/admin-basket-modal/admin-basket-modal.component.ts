import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { DataService } from 'app/data.service';
import { Router } from '@angular/router';
import { ApiService } from 'app/api.service';
import { NotificationService } from 'app/notification.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-admin-basket-modal',
  templateUrl: './admin-basket-modal.component.html',
  styleUrls: ['./admin-basket-modal.component.css']
})

export class AdminBasketModalComponent implements OnInit {

  recievedData: any;
  recievedData$:number;
  viewRationaleNomineeFirstName$:string;
  viewRationaleNomineeLastName$:string;
  managerRationale$:string;
  nominations$:any;
  nomineePayrollID$:number;
  period$:string;
  adminRationaleForm:string;

  constructor(private apiService:ApiService ,private dataService:DataService,private router: Router,
              private notifyService:NotificationService) { }
  
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
    this.nomineePayrollID$=this.recievedData.employeeId;
  }

  onSubmit(){
    //events are hardcoded ...reminder to make event dynamic
    this.apiService.adminBasketShortlistNomineesWithRationale(1,this.nomineePayrollID$,this.adminRationaleForm).subscribe(res=>{
     console.log("Shortlisted successfully");
    })
    this.notifyService.showSuccess("You have succesfully shortlist "+this.viewRationaleNomineeFirstName$+" "+this.viewRationaleNomineeLastName$,"");
     
    setTimeout(() => {
      window.location.reload();
    }, 3000);
     
  }

}
