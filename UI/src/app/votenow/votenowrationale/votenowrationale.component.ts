import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ApiService } from 'app/api.service';
import { Nominees } from 'app/model/nominees-model';
import { VotenowDataService } from '../votenow-data.service';

@Component({
  selector: 'app-votenowrationale',
  templateUrl: './votenowrationale.component.html',
  styleUrls: ['./votenowrationale.component.css']
})
export class VotenowrationaleComponent implements OnInit {

  active = 1;
  recievedData: any;
  recievedData$:number;
  viewRationaleNomineeFirstName$:string;
  viewRationaleNomineeLastName$:string;
  managerRationale$:string;
  adminRationale$:string;
  nomineeData:Nominees[]=[];
  votelistReceived:any;


  nominations$:any;
  status = ['Select Status', 'Impact', 'BeASpark'];
  selected:any;
  filtered:any;

  constructor(private apiService:ApiService,private votenowDataShareService:VotenowDataService,public activeModal: NgbActiveModal) { }

  ngOnInit(): void {

    this.votenowDataShareService.data.subscribe(
      response => {
        this.votelistReceived = response;
      });
     
      this.apiService.getShortlistedNomineesDetailsByNomineePayrollID(this.votelistReceived.nomineepayrollid).subscribe(
        response =>{
          this.recievedData=response;          
          this.recievedData.forEach(element => {
            
            this.recievedData$=element.employeeId;
            this.viewRationaleNomineeFirstName$=element.query.EmployeeFirstName;
            this.viewRationaleNomineeLastName$=element.query.EmployeeLastName;
            this.nominations$=element.nominations;
            this. managerRationale$=element.nominations.ManagerRationale;
            this.adminRationale$=element.nominations.AdminRationale;
                });


        }
      )
      
  }

}
