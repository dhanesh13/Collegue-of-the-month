import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app.routing';
import { ComponentsModule } from './components/components.module';
import { AppComponent } from './app.component';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import {MatStepperModule} from '@angular/material/stepper';
import {​​​​​​​​ OwlDateTimeModule, OwlNativeDateTimeModule }​​​​​​​​ from'ng-pick-datetime';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatSidenavModule} from '@angular/material/sidenav';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatIconModule} from '@angular/material/icon';
import {MatListModule} from '@angular/material/list';
import { FlexLayoutModule } from "@angular/flex-layout";
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { ToastrModule } from 'ngx-toastr'; // for toast message 
import { OwlModule } from 'ngx-owl-carousel';
import { HistoryComponent } from './history/history.component';
import { NgbModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { WinnerComponent } from './winner/winner.component';
import { LoginComponent } from './login/login.component';
import { DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { IgxInputGroupModule } from "igniteui-angular";
import { ChangePasswordComponent } from './change-password/change-password.component';
import { RationaleComponent } from './dashboard/rationale/rationale.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatTooltipModule } from '@angular/material/tooltip';
import { AdminBasketModalComponent } from './admin-basket/admin-basket-modal/admin-basket-modal.component';
import { ManagerRationaleModalComponent } from './manager/manager-rationale-modal/manager-rationale-modal.component';
import {MatTabsModule} from '@angular/material/tabs';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { NotFoundComponent } from './not-found/not-found.component';
import { ManagerRejectionModelComponent } from './manager/manager-rejection-model/manager-rejection-model.component';
import { ModalModule, BsModalService } from 'ngx-bootstrap/modal';
import { InspireTeamComponent } from './inspire-team/inspire-team.component';
import {MatMenuModule} from '@angular/material/menu';
import { RationaleInspireComponent } from './manager/rationale/rationale-inspire/rationale-inspire.component';
import { RationaleSparkComponent } from './manager/rationale/rationale-spark/rationale-spark.component';
import { VoucherComponent } from './voucher/voucher.component';
import { VotenowrationaleComponent } from './votenow/votenowrationale/votenowrationale.component';
import { VotenowInspireRationaleComponent } from './votenow-inspire-team/votenow-inspire-rationale/votenow-inspire-rationale.component';

//I keep the new line
@NgModule({
  imports: [
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ComponentsModule,
    OwlModule,
    MDBBootstrapModule.forRoot(),
    ModalModule.forRoot(),
    MatCheckboxModule,
    RouterModule,
    IgxInputGroupModule,
    AppRoutingModule,
    RouterModule.forRoot([
      { path: '**', redirectTo: '/404', pathMatch: 'full' } 
    ]),
    MatToolbarModule, MatButtonModule, MatSidenavModule,
    MatIconModule, MatListModule, MatStepperModule,
    MatInputModule,
    FlexLayoutModule,
    MatCardModule,
    MatAutocompleteModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-center',
      preventDuplicates: true,
      closeButton: true
    }),
    MatSnackBarModule,
    MatAutocompleteModule,
    NgbModule,
    NgbNavModule,
    MatPaginatorModule,
    MatTableModule,
    MatSortModule,
    MatTooltipModule,
    MatDatepickerModule,
    MatNativeDateModule,
    OwlDateTimeModule,
    OwlNativeDateTimeModule,
    MatFormFieldModule,
    MatTabsModule,
    MatMenuModule
  ],
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    HistoryComponent,
    WinnerComponent,
    LoginComponent,
    RationaleComponent,
    ChangePasswordComponent, 
    AdminBasketModalComponent, 
    ManagerRationaleModalComponent, 
    NotFoundComponent, 
    ManagerRejectionModelComponent, 
    InspireTeamComponent, 
    RationaleInspireComponent, 
    RationaleSparkComponent,
    VoucherComponent,
    VotenowrationaleComponent,
    VotenowInspireRationaleComponent
  ],
  providers: [DatePipe, BsModalService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
