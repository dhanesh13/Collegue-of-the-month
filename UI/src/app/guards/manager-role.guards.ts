import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { RoleService } from './role.service';
import { map } from 'rxjs/operators';
import { NotificationService } from 'app/notification.service';

@Injectable({
  providedIn: 'root'
})

export class ManagerRoleGuard implements CanActivate {

    roleStorage: number;

    constructor(public roleService: RoleService, private notifyService: NotificationService, private router: Router) {
        
    }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) : boolean {      

        this.roleStorage = Number(localStorage.getItem('role')); 

        // normal user
        if(this.roleService.role === 0 || this.roleStorage == 0) {

            this.notifyService.showError("You do not have access to this link !!", "");

            setTimeout(() => {
                this.router.navigate(['/history']);
            }, 3000); 
            return false;
        }

        // admin
        if(this.roleService.role === 2 || this.roleStorage == 2) {

            this.notifyService.showError("You do not have access to this link !!", "");

            setTimeout(() => {
                this.router.navigate(['/dashboard']);
            }, 3000); 

            return false;
        }
      
        // manager
        return true;
    }
}
