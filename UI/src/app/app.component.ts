import { Component, OnInit } from '@angular/core';
import { ApiService } from './api.service';
import { RoleService } from './guards/role.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  constructor(private apiService: ApiService, public roleService: RoleService){}

  ngOnInit(): void {
  
  }
  
}
