import { Routes } from '@angular/router';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { NominateComponent } from '../../nominate/nominate.component';
import { ManagerComponent } from '../../manager/manager.component';
import { HistoryComponent } from 'app/history/history.component';
import { WinnerComponent } from 'app/winner/winner.component';
import { AdminBasketComponent } from 'app/admin-basket/admin-basket.component';
import { AdminViewRationaleComponent } from 'app/admin-view-rationale/admin-view-rationale.component';
import { AuthGuard } from 'app/guards/auth.guard';
import { RoleGuard } from 'app/guards/role.guards';
import { ManagerRoleGuard } from 'app/guards/manager-role.guards';
import { VotingSessionComponent } from 'app/voting-session/voting-session.component';
import { DateGuard } from 'app/guards/date.guards';
import { VotenowComponent } from 'app/votenow/votenow.component';
import { UploadFileComponent } from 'app/upload-file/upload-file.component';
import { InspireTeamComponent } from 'app/inspire-team/inspire-team.component';
import { VoucherComponent } from 'app/voucher/voucher.component';
import { VotenowInspireTeamComponent } from 'app/votenow-inspire-team/votenow-inspire-team.component';

export const AdminLayoutRoutes: Routes = [
    { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard, RoleGuard] },
    { path: 'nominate', component: NominateComponent, canActivate: [AuthGuard, DateGuard] },   
    { path: 'admin-basket',  component: AdminBasketComponent, canActivate: [AuthGuard, RoleGuard] },
    { path: 'admin-view-rationale',  component: AdminViewRationaleComponent, canActivate: [AuthGuard, RoleGuard] },
    { path: 'manager',  component: ManagerComponent, canActivate: [AuthGuard, ManagerRoleGuard] },
    { path: 'history', component: HistoryComponent, canActivate: [AuthGuard] },
    { path: 'winner', component: WinnerComponent, canActivate: [AuthGuard, RoleGuard] },
    { path: 'votenow', component: VotenowComponent, canActivate: [AuthGuard, ManagerRoleGuard] },
    { path: 'votenow-inspireteam', component: VotenowInspireTeamComponent, canActivate: [AuthGuard, ManagerRoleGuard] },
    { path: 'voting-session', component: VotingSessionComponent, canActivate: [AuthGuard, RoleGuard] },
    { path: 'upload-file', component: UploadFileComponent, canActivate: [AuthGuard, RoleGuard] },
    { path: 'inspire-team', component: InspireTeamComponent, canActivate: [AuthGuard, ManagerRoleGuard, DateGuard] },
    { path: 'voucher', component: VoucherComponent, canActivate: [AuthGuard] },
];
