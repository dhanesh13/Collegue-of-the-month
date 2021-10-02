import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { RoleService } from './role.service';
import { NotificationService } from 'app/notification.service';

@Injectable({
  providedIn: 'root'
})

export class RoleGuard implements CanActivate {

    role: boolean;
    roleStorage: number;

    constructor(public roleService: RoleService, private notifyService: NotificationService, private router: Router) {
        
    }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) { 
        
        this.roleStorage = Number(localStorage.getItem('role')); 

        // normal user
        if(this.roleService.role === 0 || this.roleStorage == 0) {

            this.notifyService.showError("You do not have access to this link !!", "");

            setTimeout(() => {
                this.router.navigate(['/history']);
            }, 3000);
            return false;
        }

        // manager
        if(this.roleService.role === 1 || this.roleStorage == 1) {

            this.notifyService.showError("You do not have access to this link !!", "");

            setTimeout(() => {
                this.router.navigate(['/manager']);
            }, 3000);
            return false;
        }

        // admin
        return true;

    }
}
