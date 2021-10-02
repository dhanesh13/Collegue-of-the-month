import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from 'app/api.service';
import { DataService } from 'app/data.service';
import { NotificationService } from 'app/notification.service';

@Component({
  selector: 'app-manager-rationale-modal',
  templateUrl: './manager-rationale-modal.component.html',
  styleUrls: ['./manager-rationale-modal.component.css']
})
export class ManagerRationaleModalComponent implements OnInit {
  recievedData: any;
  recievedData$:number;
  viewRationaleNomineeFirstName$:string;
  viewRationaleNomineeLastName$:string;
  managerRationale$:string;
  nominations$:any;
  nomineePayrollID$:number;
  period$:string;

  constructor(private apiService:ApiService ,private dataService:DataService,private router: Router,private notifyService:NotificationService) { }
  managerRationaleForm:string;



  ngOnInit(): void {

  
    this.dataService.data.subscribe(
      response => {
        this.recievedData = response;
      });
      console.log(this.recievedData.employeeId);
      this.recievedData$=this.recievedData.employeeId;
      this.viewRationaleNomineeFirstName$=this.recievedData.query.EmployeeFirstName;
      this.viewRationaleNomineeLastName$=this.recievedData.query.EmployeeLastName;
      this.nominations$=this.recievedData.nominations;
      this.nomineePayrollID$=this.recievedData.employeeId;
  }

  onSubmit(){
        //events are hardcoded ...reminder to make event dynamic
    
        console.log(this.nomineePayrollID$);
        console.log(this.managerRationaleForm);
        
        this.apiService.managerShortlistNomineesWithRationale(1,this.nomineePayrollID$,this.managerRationaleForm).subscribe(res=>{
         console.log("Shortlisted successfully");
       })

       this.notifyService.showSuccess("You have Succesfully shortlist "+this.viewRationaleNomineeFirstName$+" "+this.viewRationaleNomineeLastName$,"");
       
      
       setTimeout(() => {
        window.location.reload();
      }, 5000);
       
      }

}
