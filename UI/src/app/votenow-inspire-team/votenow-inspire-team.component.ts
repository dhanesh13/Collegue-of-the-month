import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { Nominees } from 'app/model/nominees-model';
import { ApiService } from 'app/api.service';
import { DataService } from 'app/data.service';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup } from '@angular/forms';
import { VoteNow } from 'app/model/votenow-model';
import { RoleService } from 'app/guards/role.service';
import { VoteLogNominations } from 'app/model/votelog-nominations';
import { AuthService } from 'app/guards/auth.service';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { NotificationService } from 'app/notification.service';
import { VotenowDataService } from 'app/votenow/votenow-data.service';
import { VotenowrationaleComponent } from 'app/votenow/votenowrationale/votenowrationale.component';

@Component({
  selector: 'app-votenow-inspire-team',
  templateUrl: './votenow-inspire-team.component.html',
  styleUrls: ['./votenow-inspire-team.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class VotenowInspireTeamComponent implements OnInit {

  displayedColumns: string[] = ['TeamName', 'Rationales', 'select'];

  dataSource;

  managerId: number;

  selection;

  @ViewChild('TableVoteLogPaginator', {static: true}) tableVoteLogPaginator: MatPaginator;
  @ViewChild('TableVoteLogSort', {static: true}) tableVoteLogSort: MatSort;

  public topNominees: Nominees[];
  public topVoteLogNominations: VoteLogNominations[];

  form: FormGroup;

  closeResult: string;
  nominationPeriod;
  idmanager2: string;
  managerID: number;
  public activesessionID: number;

  voteNowList: VoteNow[] = [];
  list: VoteNow[] = [];
  voted : VoteNow[] = [];

  searchKey: string;

  isTableHasData = true;

  public voteNowSelected: VoteNow[] = [];
  voteNowIndex: any;
  period;

  constructor(private apiService: ApiService, private votenowDataShareService: VotenowDataService, private roleService: RoleService, 
    private modalService: NgbModal, public authService: AuthService, private notifyService: NotificationService) { }

  ngOnInit(): void {

    this.managerId = Number(localStorage.getItem('managerId'));
    
    this.period = String(localStorage.getItem('period'));

    this.apiService.getActiveCOTMVotingSessionId().subscribe(sessionid =>
      {
        this.activesessionID = Number(sessionid);
        
        localStorage.setItem('activeSessionId', this.activesessionID.toString());
        
      });

    this.roleService.mId$.subscribe(
      response => {

        this.idmanager2 = response;
      });
    
    this.getVoteLog();

    // get current nomination period
    this.apiService.getPeriod()
      .subscribe((data) => {

        this.nominationPeriod = data;
      });
  }

  onSearchClear()
  {
    this.searchKey = "";
    this.applyFilter();
  }

  applyFilter()
  {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
    if(this.dataSource.filteredData.length > 0){
      this.isTableHasData = true;
    } else {
      this.isTableHasData = false;
    }
  }

  retrieveTopNominees(): void {

    this.apiService.getTopShortlistedNomineesDetailsForVote()
      .subscribe(
        (data: Nominees[]) => {
          this.topNominees = data;      
          
          // filtering based on name
          this.dataSource.filterPredicate = function(data: any, filterValue: string) {
            return data.TeamName.trim().toLocaleLowerCase().indexOf(filterValue.trim().toLocaleLowerCase()) >= 0;
          };
        },
        error => {
          console.log(error);
        });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  onClickViewRationale(votelist: VoteNow) {
    
    this.modalService.open(VotenowrationaleComponent, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });

   this.votenowDataShareService.setData(votelist);

  }

  call() {

    this.voteNowList.length = 0;

    for(let nominee of this.list){
      this.voted.forEach(element => {
        if(element.nomineepayrollid==nominee.nomineepayrollid && element.voted==true ){
          nominee.voted==true;
        }        
      });
    }

    for (let nominee of this.list) {
      let model = new VoteNow();
      model.voted = nominee.voted;
      model.nomineepayrollid = nominee.nomineepayrollid;
      model.managerpayrollid = nominee.managerpayrollid;
      model.sessionid=nominee.sessionid;
      model.name=nominee.name;
      model.period = nominee.period
      model.eventid = nominee.eventid;
      this.voteNowList.push(model);
    }  

    this.apiService.postVoteNow(this.voteNowList).subscribe(x => {     
    });

    this.notifyService.showSuccess("You have successfully voted  !!", "");
    
    setTimeout(() => {
      window.location.reload();
    }, 3000);

  }

  getVoteLog(): void {
    
    this.apiService.getVoteLogByManager(1)

      .subscribe(
        (data: VoteLogNominations[]) => {

          this.topVoteLogNominations = data;

          //@ts-ignore        
          if(!this.topVoteLogNominations.voteLogs || !this.topVoteLogNominations.voteLogs.length){
            //@ts-ignore
            this.topVoteLogNominations.nominations.forEach(element => {
            
              let templist = new VoteNow();
              templist.nomineepayrollid = element.employeeId;
              templist.managerpayrollid= this.managerId;
              templist.eventid = 1;//cotm
              templist.period = this.period;
              templist.voted = false;
              templist.sessionid = this.activesessionID;
              templist.name = element.query.employeeFirstName + " " + element.query.employeeLastName;
              this.list.push(templist);
              
            });
          }
          else
          {
            //@ts-ignore
            this.topVoteLogNominations.voteLogs.forEach(element => { 

              let templist = new VoteNow();
              templist.nomineepayrollid = element.nomineepayrollid;
              templist.managerpayrollid = element.managerpayrollid;
              templist.eventid = element.eventid;
              templist.period = element.period;
              templist.voted = element.voted;
              templist.sessionid = element.sessionid;
            
              //@ts-ignore
              var xx = this.topVoteLogNominations.nominations.filter(n => n.employeeId == element.nomineepayrollid);
              templist.name = xx[0].query.employeeFirstName + " " + xx[0].query.employeeLastName;
              this.list.push(templist);
            });
          }

          this.dataSource = new MatTableDataSource(this.list);this.dataSource.sortingDataAccessor = (item, property) => {
            if (property.includes('.')) return property.split('.').reduce((o,i)=>o[i], item)
            return item[property];
          };

          this.dataSource.paginator = this.tableVoteLogPaginator;
          this.dataSource.sort = this.tableVoteLogSort;
           },
            error => {
              console.log(error);
      });

  }

  toggle(item,event: MatCheckboxChange) 
  {
    if (event.checked)
    {
      item.voted=true;
      this.voted.push(item);
    } 
    else 
    {
      const index = this.voted.indexOf(item);

      if (index >= 0) 
      {
        this.voted.splice(index, 1);
      }

      item.voted = false;
      this.voted.push(item);
    }  
  }

  exists(item) 
  {
    return this.voted.indexOf(item) > -1;
  }

  isChecked() {
    return this.voted.length === this.list.length;
  }

  toggleAll(event: MatCheckboxChange) 
  { 
    if (event.checked) 
    {
      this.list.forEach(row => {

        row.voted=true;
        this.voted.push(row)
      });
    } 
    else 
    {
      this.voted.length = 0 ;
    }
  }

  isIndeterminate() {
    return (this.voted.length > 0 && !this.isChecked());
  }
}

