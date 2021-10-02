import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { DateService } from './date.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Injectable({
  providedIn: 'root'
})

export class DateGuard implements CanActivate {

    nominationPeriodStorage: string;
    roleStorage: number;

    constructor(public dateService: DateService, private router: Router) {
        
    }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) { 
        
        this.nominationPeriodStorage = localStorage.getItem('nominationPeriod');
        this.roleStorage = Number(localStorage.getItem('role'));  

        // if(this.dateService.dateToVote == false) {
        //     Swal.fire('', 'Sorry, nomination is currently closed!', 'error');
        //     return false;
        // }

        // // admin
        // if(this.nominationPeriodStorage == 'false' && this.roleStorage == 2) {
        //     Swal.fire('', 'Sorry, nomination is currently closed!', 'error');

        //     setTimeout(() => {
        //         this.router.navigate(['/dashboard']);
        //     }, 2000); 

        //     return false;
        // }

        // // manager
        // if(this.nominationPeriodStorage == 'false' && this.roleStorage == 1) {
        //     Swal.fire('', 'Sorry, nomination is currently closed!', 'error');

        //     setTimeout(() => {
        //         this.router.navigate(['/manager']);
        //     }, 2000); 

        //     return false;
        // }

        // // user
        // if(this.nominationPeriodStorage == 'false' && this.roleStorage == 0) {
        //     Swal.fire('', 'Sorry, nomination is currently closed!', 'error');

        //     setTimeout(() => {
        //         this.router.navigate(['/history']);
        //     }, 2000); 

        //     return false;
        // }
        return true;
    }
}
