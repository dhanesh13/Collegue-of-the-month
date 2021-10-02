import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from 'app/api.service';
import { EventModel } from 'app/model/event';
import { EventVotingSession } from 'app/model/event-voting-session';
import { VotingSession } from 'app/model/voting-session-model';
import { NotificationService } from 'app/notification.service';
import { DatePipe } from '@angular/common';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';

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
  selector: 'app-edit-session',
  templateUrl: './edit-session.component.html',
  styleUrls: ['./edit-session.component.css'],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ]
})
export class EditSessionComponent implements OnInit {

  public events: EventModel[];
  public eventVotingSessions: EventVotingSession[];
  public test: string;
  public counter: number;

  form: FormGroup;

  eventId;
  sessionId;

  minOpeningDate: Date;
  maxOpeningDate: Date;
  minClosingDate: Date;
  maxClosingDate: Date;

  editVotingSessionForm: VotingSession = new VotingSession();

  constructor(public dialogRef: MatDialogRef<EditSessionComponent>, private formBuilder: FormBuilder,
    private apiService: ApiService, @Inject(MAT_DIALOG_DATA) public data: EventVotingSession,
    public datepipe: DatePipe,
    private notifyService: NotificationService
  ) {
    const currentYear = new Date().getFullYear();
    this.minOpeningDate = this.data.openingDate;
    this.maxOpeningDate = new Date(currentYear + 1, 11, 31);
    this.minClosingDate = new Date();
    this.maxClosingDate = new Date(currentYear + 1, 11, 31);
   }

  ngOnInit(): void {

    this.eventList();

    this.form = this.formBuilder.group(
      {
        openingDate: ['', [Validators.required]],
        closingDate: ['', [Validators.required]],
        eventEdit: ['', [Validators.required]],
      }
    )


    // this.form.get('eventEdit').valueChanges
    //   .subscribe(data => {
    //     this.eventId = data;
    //   });

      this.form.get('eventEdit').valueChanges
      .subscribe(data => {
        if (data) {
          this.eventId = data;
        }
      });

    this.form.get("openingDate").setValue(this.data.openingDate);
    this.form.get("closingDate").setValue(this.data.closingDate);
    this.form.get("eventEdit").setValue(this.data.eventId);

  }

  eventList(): void {
    this.apiService.getEvents()
      .subscribe(
        (data: EventModel[]) => {
          this.events = data;
          console.log(data);
        },
        error => {
          console.log(error);
        });
  }

  updateVotingSession(): void {
    this.editVotingSessionForm.eventId = this.eventId;
    this.editVotingSessionForm.OpenDate = this.datepipe.transform(this.form.get('openingDate').value, 'yyyy-MM-dd');
    this.editVotingSessionForm.CloseDate = this.datepipe.transform(this.form.get('closingDate').value, 'yyyy-MM-dd');

    console.log(this.editVotingSessionForm);

    if (this.form.invalid) {
      this.notifyService.showError("All fields are required! Please enter the correct details!", "");
    }
    else if (this.editVotingSessionForm.OpenDate > this.editVotingSessionForm.CloseDate) {
      this.notifyService.showError("Opening Date cannot be greater than Closing Date!", "");
    }
    // else if (this.editVotingSessionForm.CloseDate < new Date()) {
    //   this.notifyService.showError("Closing Date cannot be less than Today's date - "
    //     + " " + this.datepipe.transform(new Date(), 'dd/MM/yyyy') + " !", "");
    // }
    else {
      this.apiService.editVotingSession(this.data.sessionId, this.editVotingSessionForm)
        .subscribe(
          response => {
            console.log(response);
            if(response == true){          
              this.notifyService.showSuccess("Voting Session updated successfully!", "");
              this.dialogRef.close();
              }
              else{
                this.notifyService.showError("Session already exists!", "");               
              }               
          },
          error => {
            console.log(error);
          });
    }
   }

}
