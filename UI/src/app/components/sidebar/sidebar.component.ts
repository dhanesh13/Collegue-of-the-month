import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { AuthService } from 'app/guards/auth.service';
import { RoleService } from 'app/guards/role.service';
import { Router } from '@angular/router';
import { ApiService } from 'app/api.service';

declare const $: any;
declare interface RouteInfo {
    path?: string;
    title: string;
    icon: string;
    class: string;
    flg: any[];
    children: any[];
}

// normal user
export const ROUTES: RouteInfo[] = [
    { path: '/nominate', title: 'Nominate',  icon:'person_add_alt_1', class: '' , flg: [], children: [] },
    { path: '/history', title: 'History',  icon:'history', class: '', flg: [], children: []  },
];

// manager
export const ROUTES1: RouteInfo[] = [
  { path: '/nominate', title: 'Nominate',  icon:'person_add_alt_1', class: '', flg: [{ "SHOW_ICON": "YES" }],
    children: [
      { path: '/nominate', title: 'COTM',  icon: 'person', class: '' },
      { path: '/inspire-team', title: 'Inspire Team',  icon:'groups', class: ''},
    ]},
  { path: '/manager', title: 'Dashboard',  icon:'dashboard', class: '', flg: [{ "SHOW_ICON": "NO" }], children: []  },
  { path: '/history', title: 'History',  icon:'history', class: '' , flg: [{ "SHOW_ICON": "NO" }], children: [] }
];

export const ROUTESVOTENOW: RouteInfo[] = [
  { path: '/votenow', title: 'VoteNow',  icon:'how_to_reg', class: '' , flg: [{ "SHOW_ICON": "YES" }], 
    children: [
      { path: '/votenow', title: 'COTM',  icon: 'person', class: '' },
      { path: '/votenow-inspireteam', title: 'Inspire Team',  icon:'groups', class: ''},
    ]}
];

// admin
export const ROUTES2: RouteInfo[] = [
  { path: '/dashboard', title: 'Dashboard',  icon: 'dashboard', class: '', flg: [], children: []  },
  { path: '/nominate', title: 'Nominate',  icon:'person_add_alt_1', class: '' , flg: [], children: [] },
  { path: '/admin-basket', title: 'Admin Basket',  icon:'shopping_basket', class: '', flg: [], children: [] },
  { path: '/voting-session', title: 'Voting Session', icon: 'how_to_vote', class: '', flg: [], children: [] },
  { path: '/winner', title: 'Winner', icon: 'emoji_events', class: '' , flg: [], children: [] },
  { path: '/history', title: 'History', icon: 'history', class: '' , flg: [], children: [] },
  { path: '/upload-file', title: 'Update Employee List', icon: 'upload_file', class: '', flg: [], children: [] }
];

// voucher
export const ROUTESVOUCHER: RouteInfo[] = [
  { path: '/voucher', title: 'Voucher',  icon:'redeem', class: '' , flg: [], children: []}
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];
  menuItems2: any[];
  menuItems3: any[];
  menuItemsVoteNow: any[];
  menuItemsVoucher: any[];

  roleUser: number;
  votenowStatus:boolean;

  showVoucher: boolean;

  show = -1;
  submenu;

  showVotenow = -1;
  submenuVotenow;

  constructor(public authService: AuthService, public roleService: RoleService, private router: Router,
              public apiService: ApiService) { }

  ngOnInit() {
    // get current voting session to display votenow in sidebar if session open
    this.authService.getVotingSession()
    .subscribe((data) => {
    
      if(data == true)
      {
        this.votenowStatus = true;       
      }
    
      if(data == false)
      {
        this.votenowStatus = false;
      }
      
    });

    // show voucher in sidebar if winner
    this.apiService.getShowVoucher()
    .subscribe((data) => {
    
      if(data == true)
      {
        this.showVoucher = true;       
      }
     
      if(data == false)
      {
        this.showVoucher = false;
      }
      
    });


    this.authService.getAccess()
      .subscribe(() => {     
    }); 
  
    this.roleUser = Number(localStorage.getItem('role'));

    this.menuItems = ROUTES.filter(menuItem => menuItem);
    this.menuItems2 = ROUTES1.filter(menuItem => menuItem);
    this.menuItems3 = ROUTES2.filter(menuItem => menuItem);
    this.menuItemsVoteNow = ROUTESVOTENOW.filter(menuItem => menuItem);
    this.menuItemsVoucher = ROUTESVOUCHER.filter(menuItem => menuItem);
  }

  showsubmenu(index) {
    
    this.submenu = this.menuItems2[index]["title"];
  }

  // votenow
  showsubmenuVotenow(index) {
    
    this.submenuVotenow = this.menuItemsVoteNow[index]["title"];
  }

  isMobileMenu() {
    if ($(window).width() > 991) {
        return false;
    }
    return true;
  };

  logOut()
  {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('role');
    localStorage.removeItem('managerId');
    localStorage.removeItem('nominationPeriod');
    localStorage.removeItem('period');
    localStorage.removeItem('voterPayrollId');
    localStorage.removeItem('activeSessionId');
    this.router.navigate(['/login']); 
    
  }
}
