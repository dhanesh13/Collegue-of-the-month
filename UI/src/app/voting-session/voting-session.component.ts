import { Component, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ApiService } from 'app/api.service';
import { VotingSession } from 'app/model/voting-session-model';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NotificationService } from 'app/notification.service';
import { TooltipPosition } from '@angular/material/tooltip';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EditSessionComponent } from 'app/voting-session/edit-session/edit-session.component';
import { DatePipe } from '@angular/common';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';
import { CloseSessionConfirmationComponent } from './close-session-confirmation/close-session-confirmation.component';
import { EventModel } from 'app/model/event';
import { EventVotingSession } from 'app/model/event-voting-session';

export const MY_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'DD/MM/YYYY',
    monthYearLabel: 'YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'YYYY',
  },
};

@Component({
  selector: 'app-voting-session',
  templateUrl: './voting-session.component.html',
  styleUrls: ['./voting-session.component.css'],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
  encapsulation: ViewEncapsulation.None
})
export class VotingSessionComponent implements OnInit {

  position: TooltipPosition = 'after';

  displayedColumns: string[] = ['eventName', 'eventDescription', 'period', 'openingDate', 'closingDate', 'actions'];
  dataSource;
  period;
  nominationPeriod;

  minOpeningDate: Date;
  maxOpeningDate: Date;
  minClosingDate: Date;
  maxClosingDate: Date;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  public events: EventModel[];
  public eventVotingSessions: EventVotingSession[];

  eventId;

  form: FormGroup;
  votingSessionForm: VotingSession = new VotingSession();

  editSessionForm: EventVotingSession = new EventVotingSession();


  constructor(private formBuilder: FormBuilder, private apiService: ApiService,
    public dialog: MatDialog,
    public datepipe: DatePipe,
    private notifyService: NotificationService) {

    const currentYear = new Date().getFullYear();
    this.minOpeningDate = new Date();
    this.maxOpeningDate = new Date(currentYear + 1, 11, 31);
    this.minClosingDate = new Date();
    this.maxClosingDate = new Date(currentYear + 1, 11, 31);
  }

  openEditDialog(eventVotingSessions: EventVotingSession): void {
    const dialogRef = this.dialog.open(EditSessionComponent, {
      data: {
        eventId: eventVotingSessions.eventId, sessionId: eventVotingSessions.sessionId, openingDate: eventVotingSessions.openingDate, closingDate: eventVotingSessions.closingDate,
        eventDescription: eventVotingSessions.eventDescription
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.sessionList();
      console.log('The dialog was closed');
    });
  }

  closeSessionDialog(eventVotingSessions: EventVotingSession): void {
    const dialogRef = this.dialog.open(CloseSessionConfirmationComponent, {
      data: {
        eventId: eventVotingSessions.eventId, sessionId: eventVotingSessions.sessionId, openingDate: eventVotingSessions.openingDate, closingDate: eventVotingSessions.closingDate,
        eventDescription: eventVotingSessions.eventDescription
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.sessionList();
      console.log('The dialog was closed');
    });
  }



  ngOnInit(): void {
    this.statusClosingDate();
    this.eventList();
    this.sessionList();

    this.form = this.formBuilder.group(
      {
        openingDate: ['', [Validators.required]],
        closingDate: ['', [Validators.required]],
        event: ['', [Validators.required]],
      }
    )

    this.form.get('event').valueChanges
      .subscribe(data => {
        if (data) {
          this.eventId = data.eventId;
        }
      });

    // get current nomination period
    this.apiService.getPeriod()
    .subscribe((data) => {

      this.nominationPeriod = data;
    });
  }


  saveVotingSession() {

    this.votingSessionForm.period = this.getPeriod();
    this.votingSessionForm.eventId = this.eventId;
    this.votingSessionForm.OpenDate = this.datepipe.transform(this.form.get('openingDate').value, 'yyyy-MM-dd');
    this.votingSessionForm.CloseDate = this.datepipe.transform(this.form.get('closingDate').value, 'yyyy-MM-dd');
    this.votingSessionForm.status = true;

    if (this.form.invalid) {
      this.notifyService.showError("All fields are required! Please enter the correct details!", "");
    }
    else if (this.votingSessionForm.OpenDate > this.votingSessionForm.CloseDate) {
      this.notifyService.showError("Opening Date cannot be greater than Closing Date!", "");
    }
    else {
      this.apiService.createVotingSession(this.votingSessionForm)
        .subscribe(
          response => {
            console.log(this.votingSessionForm);
            console.log(response);
            if(response == true){          
            this.notifyService.showSuccess("Voting Session created successfully!", "");
            //this.form.reset();
            this.sessionList();
           // window.location.reload();
            }
            
            else{
              this.notifyService.showError("Session already exists!", "");
              this.sessionList();
            }  
            if(response == true && this.votingSessionForm.eventId == 1){
              console.log("Test");
              this.apiService.sendEmailForVotingSessionToManagers().subscribe( error => {
                console.log(error);
              });
            }         
          },
          error => {
            console.log(error);
          });
    }
    

  }

  eventList(): void {
    this.apiService.getEvents()
      .subscribe(
        (data: EventModel[]) => {
          this.events = data;
        },
        error => {
          console.log(error);
        });
  }

  sessionList(): void {
    this.apiService.getVotingSessions()
      .subscribe(
        (data: EventVotingSession[]) => {
          this.eventVotingSessions = data;
          this.dataSource = new MatTableDataSource(this.eventVotingSessions);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        },
        error => {
          console.log(error);
        });
  }

  getPeriod() {
    var months = new Array("January", "February", "March", "April", "May", "June",
      "July", "August", "September", "October", "November", "December");

    var todayDate = new Date();
    var monthName = months[todayDate.getMonth()];
    var year = todayDate.getFullYear();
    this.period = monthName + year;
    return this.period;
  }

  clearVotingSession(): void {
    this.form.reset();
  }

  statusClosingDate(): void {
    this.apiService.getStatusClosingDate()
      .subscribe(
        data => {
        },
        error => {
          console.log(error);
        });
  }

}

