import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from 'app/api.service';
import { Login } from 'app/model/login';
import { AuthService } from 'app/guards/auth.service';
import { RoleService } from 'app/guards/role.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { DateService } from 'app/guards/date.service';
import { NotificationService } from 'app/notification.service';
import { VotingSessionService } from 'app/guards/voting-session.service';
import { EventVotingSession } from 'app/model/event-voting-session';
import {MatSnackBar} from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  date : Date = new Date();
  triggerMail= false

  form: FormGroup;
  formUsername: string;
  formPassword: string;

  loginForm: Login = new Login();

  item:EventVotingSession;

  public eventVotingSessions: EventVotingSession[];
  nominationPeriod;
  public activesessionID: number;

  constructor(private formBuilder: FormBuilder, private router: Router, private service: ApiService, 
    public authService: AuthService, public roleService: RoleService, public dateService: DateService,
    private notifyService: NotificationService, public votingSessionService: VotingSessionService,
    private _snackBar: MatSnackBar) {

    this.formUsername = 'username';
    this.formPassword = 'password';

    this.form = this.formBuilder.group(
      {
        username: ['', [Validators.required]],
        password: ['', [Validators.required]],
      }
    )
  }

  ngOnInit(): void {
    this.statusClosingDate();
  }

  // Getters
  get username() { return this.form.get(this.formUsername); }
  get password() { return this.form.get(this.formPassword); } 

  save() {

    if (!this.form.valid) {
      this.notifyService.showError("All fields are required! Please enter the correct details!", "");
      return;
    }
  
    this.loginForm.username = this.form.get(this.formUsername).value;
    this.loginForm.password = this.form.get(this.formPassword).value;

    this.authService.userLogin(this.loginForm)
      .subscribe((data) => {

        // if invalid username or password entered
        if(data.isAuth == false)
        {
          this.notifyService.showError("Invalid data! Please enter the correct details!", "");
        }

        // valid username and password
        else if(data.isAuth == true)
        {
          this.roleService.setEmpRole(data.employeeRole);        
          this.roleService.setEmployeeId(data.payrollID);  
          
          /**  for session */
          localStorage.setItem('currentUser', this.username.value); 
          localStorage.setItem('role', data.employeeRole.toString());
          localStorage.setItem('voterPayrollId', data.payrollID.toString());

          // get active COTM VotingSessionId for session
          this.service.getActiveCOTMVotingSessionId().subscribe(sessionid=>
          {
            this.activesessionID=Number(sessionid);
            
            localStorage.setItem('activeSessionId', this.activesessionID.toString());
            
          });
          
          // get current nomination period for session
          this.service.getPeriod()
          .subscribe((period) => {
            if (period){
              localStorage.setItem('period', period); 
              
              // normal user
              if(data.employeeRole == 0)
              {
                this.authService.getAccess()
                .subscribe((dataDate) => {

                  this.dateService.setDate(dataDate);
                  localStorage.setItem('nominationPeriod', dataDate.toString());  

                  this._snackBar.open("Welcome " + data.employeeFirstName, " ", {
                    duration: 3000,
                    panelClass: ['purple-snackbar']
                  });

                  this.roleService.setManagerId(data.managerId);
                    
                  if(dataDate == false)
                  {             
                    this.router.navigate(['/history']); 
                  }
                  else{              
                    this.router.navigate(['/nominate']); 
                  }
                }); 
              }

              // manager
              if(data.employeeRole == 1)
              {
                this.authService.getAccess()
                .subscribe((dataDate) => {

                  this.dateService.setDate(dataDate);
                  localStorage.setItem('nominationPeriod', dataDate.toString()); 

                }); 
                
                /**  for session */
                localStorage.setItem('managerId',data.payrollID.toString()); 

                this.roleService.setManagerId(data.payrollID);
                this.router.navigate(['/manager']);   
              
                this._snackBar.open("Welcome " + data.employeeFirstName, " ", {
                  duration: 3000,
                  panelClass: ['purple-snackbar']
                });             
              }
              
              // admin
              if(data.employeeRole == 2)
              {
                this.authService.getAccess()
                .subscribe((dataDate) => {

                  this.dateService.setDate(dataDate);
                  localStorage.setItem('nominationPeriod', dataDate.toString());
                  this.service.checkEmailForCOTMEvent().subscribe((checkEmailCOTMSent)=>{
                    // console.log(dataDate);
                      if(!dataDate && !checkEmailCOTMSent){
                        console.log("cotm closed");
                      }
                      else if(dataDate && !checkEmailCOTMSent){
                        this.service.sendEmailForCOTMEventWhenOpen().subscribe();
                       this.service.updateMailAfterEmailSent().subscribe();
                       console.log("we just sent your email");
                        
                      }
                      else if(dataDate && checkEmailCOTMSent){
                        console.log("email already sent");

                      }
                  }) ;
                
                }); 
                this.authService.getAccessShortlistPeriod()
                .subscribe((shortlistPeriod)=>{
               
                  this.service.checkEmailForShortlistPeriod().subscribe((checkEmailShortlistPeriodSent)=>{
                      if(shortlistPeriod && !checkEmailShortlistPeriodSent){
                        this.service.sendEmailForShortlistPeriodOpen().subscribe();
                       this.service.updateMailSentWhenShortlistPeriodOpen().subscribe();
                       
                      }                                       
                  }) ;
                });

                this.roleService.setManagerId(data.managerId);
                this.router.navigate(['/dashboard']);
                
                this._snackBar.open("Welcome " + data.employeeFirstName, " ", {
                  duration: 3000,
                  panelClass: ['purple-snackbar']
                });
              }
            }
          });
                
          
         
        }
      });   
  }

  statusClosingDate(): void {
    this.service.getStatusClosingDate()
      .subscribe(
        data => {
        },
        error => {
          console.log(error);
        });
  }
}
