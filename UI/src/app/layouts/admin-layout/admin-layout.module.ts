import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminLayoutRoutes } from './admin-layout.routing';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import {MatRippleModule} from '@angular/material/core';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatSelectModule} from '@angular/material/select';
import {MatStepperModule} from '@angular/material/stepper';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatRadioModule} from '@angular/material/radio';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {MatTabsModule} from '@angular/material/tabs';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatDividerModule} from '@angular/material/divider';
import {MatTableModule} from '@angular/material/table';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatSortModule} from '@angular/material/sort';
import {MatIconModule} from '@angular/material/icon';
import {MatDialogModule} from '@angular/material/dialog';
import { NominateComponent } from '../../nominate/nominate.component';

import { 
	IgxDropDownModule,
	IgxInputGroupModule,
	IgxRippleModule,
	IgxIconModule,
	IgxToggleModule
 } from "igniteui-angular";
import { ManagerComponent } from '../../manager/manager.component';
import { VotenowComponent } from 'app/votenow/votenow.component';
import { NgxPaginationModule } from 'ngx-pagination';

import { CdkColumnDef } from '@angular/cdk/table';
import { AdminBasketComponent } from 'app/admin-basket/admin-basket.component';
import { VotingSessionComponent } from 'app/voting-session/voting-session.component';
import { EditSessionComponent } from 'app/voting-session/edit-session/edit-session.component';
import { CloseSessionConfirmationComponent } from 'app/voting-session/close-session-confirmation/close-session-confirmation.component';
import { UploadFileComponent } from 'app/upload-file/upload-file.component';
import { VotenowInspireTeamComponent } from 'app/votenow-inspire-team/votenow-inspire-team.component';



@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatRippleModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTooltipModule,
    MatStepperModule,
    MatAutocompleteModule, 
    MatCheckboxModule,
    MatRadioModule,
    MatTabsModule,
    MatDatepickerModule,
    MatDividerModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatIconModule,
    MatDialogModule,
    IgxDropDownModule,
    IgxInputGroupModule,
    IgxRippleModule,
    IgxIconModule,
    IgxToggleModule,
    NgbModule,
    NgxPaginationModule
  ],
  declarations: [
    DashboardComponent,
    NominateComponent,
    AdminBasketComponent,
    ManagerComponent,
    VotingSessionComponent,
    EditSessionComponent,
    VotenowComponent,
    VotenowInspireTeamComponent,
    CloseSessionConfirmationComponent,
    UploadFileComponent
  ],
  exports: [ MatFormFieldModule, MatInputModule, MatTableModule ],
  providers:[CdkColumnDef]
})

export class AdminLayoutModule {}
