import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'app/api.service';
import { DataService } from 'app/data.service';
import { NotificationService } from 'app/notification.service';

@Component({
  selector: 'app-manager-rejection-model',
  templateUrl: './manager-rejection-model.component.html',
  styleUrls: ['./manager-rejection-model.component.css']
})
export class ManagerRejectionModelComponent implements OnInit {

  recievedData: any;
  recievedData$:number;
  viewRationaleNomineeFirstName$:string;
  viewRationaleNomineeLastName$:string;
  managerRationale$:string;
  nominations$:any;
  nomineePayrollID$:number;
  period$:string;

  constructor(private apiService:ApiService ,private dataService:DataService,private router: Router,private notifyService:NotificationService) { }
  managerRejectionForm:string;

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
        console.log(this.managerRejectionForm);
        
        this.apiService.managerRejectionRationale(1,this.nomineePayrollID$,this.managerRejectionForm).subscribe(res=>{
         console.log("Shortlisted successfully");
       })

       this.notifyService.showError("Rejection rationale added for "+this.viewRationaleNomineeFirstName$+" "+this.viewRationaleNomineeLastName$,"");
       
      
       setTimeout(() => {
        window.location.reload();
      }, 5000);
       
      }
  }


