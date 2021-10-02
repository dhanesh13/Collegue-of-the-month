import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule  } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { LoginComponent } from './login/login.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { NotFoundComponent } from './not-found/not-found.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login', 
    pathMatch: 'full',
  }, 
  {
    path: '',
    component: AdminLayoutComponent,
    children: [{
      path: '',
      loadChildren: './layouts/admin-layout/admin-layout.module#AdminLayoutModule'
    }]
  },
  { path: 'login', component: LoginComponent},
  { path: 'change-password', component: ChangePasswordComponent},
  { path: '404', component: NotFoundComponent},
];

@NgModule({
  imports: [
    CommonModule,
    
    BrowserModule,
    RouterModule.forRoot(routes,{
       useHash: true,
       anchorScrolling: 'enabled'
    })
  ],
  exports: [
  ],
})
export class AppRoutingModule { }
