import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApiService } from 'app/api.service';
import { EventVotingSession } from 'app/model/event-voting-session';
import { NotificationService } from 'app/notification.service';

@Component({
  selector: 'app-close-session-confirmation',
  templateUrl: './close-session-confirmation.component.html',
  styleUrls: ['./close-session-confirmation.component.css']
})
export class CloseSessionConfirmationComponent implements OnInit {

  public eventVotingSessions: EventVotingSession[];

  constructor(public dialogRef: MatDialogRef<CloseSessionConfirmationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EventVotingSession, private apiService: ApiService,
    private notifyService: NotificationService) { }

  ngOnInit(): void {
  }

  closeSession() {
    this.apiService.closeVotingSession(this.data.sessionId)
      .subscribe(
        response => {
          console.log(response);
          this.notifyService.showSuccess("Voting Session closed successfully!", "");
          this.dialogRef.close();
        },
        error => {
          console.log(error);
        });
  }

}
