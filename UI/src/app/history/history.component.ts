import { Component, OnInit, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { ApiService } from 'app/api.service';
import { HallOfFameWinners } from 'app/model/hall-of-fame-winners';

export interface WinnersList {
  title: string;
  description: string;
  rationale: string;
  img: string;
}

export interface WinnerList {
  FirstName: string;
  LastName: string;
  Period: string;
  Rationale:string;
}

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css'],
  encapsulation: ViewEncapsulation.None,
  providers: [NgbCarouselConfig]  // add NgbCarouselConfig to the component providers

})
export class HistoryComponent implements OnInit {

  form: FormGroup;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['fullName', 'period', 'viewRationale'];
  dataSource;

  searchKey: string; 

  fullName: string;
  description: string;
  period: string;

  isTableHasData = true;

  firstName: string;
  lastName: string;

  public hallOfFameWinners: HallOfFameWinners[];

  @ViewChild('callAPIDialog') callAPIDialog: TemplateRef<any>;

  constructor(public dialog: MatDialog, private formBuilder: FormBuilder, private apiService: ApiService) {
  }

  openRationaleDialog(winnersList: WinnersList): void {
    const dialogRef = this.dialog.open(this.callAPIDialog, {
      data: {},
      width: '600px',
    });

    this.fullName= winnersList.description;
    this.period= winnersList.title;
    this.form.get("rationale").setValue(winnersList.rationale);

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  openRationaleModal(winnersList: WinnerList): void {
    const dialogRef = this.dialog.open(this.callAPIDialog, {
      data: {},
      width: '600px',
    });

    this.fullName= winnersList.FirstName +' '+ winnersList.LastName;
    this.period= winnersList.Period;
    this.form.get("rationale").setValue(winnersList.Rationale);

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  ngAfterViewInit() {
    
  }

  ngOnInit(): void {


    this.apiService.getHallOfFameWinner()
      .subscribe(data =>{
        console.log(data);
        this.bslide = this.chunk(data,4);
        console.log(this.bslide)
      });

    this.slides = this.chunk(this.cards, 4);
    this.aslide = this.chunk(this.acard, 4);

    this.form = this.formBuilder.group(
      {
        rationale: ['', [Validators.required]]
      }
    )
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
  
cards = [
  {
    title: 'January',
    description: 'Ilesh Babeea',
    rationale:'Being part of the DIKA Support squad, Ilesh has had numerous interactions with various internal stakeholders within the group. Recently, he was praised for his patience and efficiency in dealing with several issues for a particular user. Ilesh has been very focused and he ensured that due attention was given to our internal customers. He has indeed contributed to one of the strategic pillars: Customer Centricity!',
    img: 'assets/img/100050.jpg'
  },
  {
    title: 'February',
    description: 'Nikhil Ramma',
    rationale:'Nikhil has implemented new ways of working in terms of MIM and problem management in Application Support . So far Nikhil has successfully rolled out his methodology getting the key stakeholders onboard while also delivering some proof of concepts in terms of Problem management and defect fixing for our key customers M&S, Whitbread and non-corporates. On the MIM side a decrease in the volume of MIs can be noted allowing the BAU teams to focus on improving their key measures and KPIs.​',
    img: 'assets/img/200502.jpg'
  },
  {
    title: 'March',
    description: 'Bhavna Gungea',
    rationale:'Bhavna has been consistently receiving very good feedback from his customers on the level of service delivered. She is very patient and handle the customer incident/ call very well. She will always go the extra miles. Example of a feedback from the customer: “As always very efficient, helpful and cheerful, even in these uncertain, difficult times.​',
    img: 'assets/img/201762.jpg'
  },
  {
    title: 'April',
    description: 'Nasir Lollmahamod ',
    rationale:'Naasir has been instrumental in the successful realisation of the Furlough tasks for the Customer Marks & Spencer. This was a new task with a very tight deadline. Naasir came up with new ideas and methods which helped to deliver before the deadlines. He worked intensively and efficiently on this project without impacting the BAU activities. ​Higher management from both SD Worx and M&S (including Andrew Dresner) were very pleased with the level of commitment and dedication Naasir gave to this project and make it a success.​',
    img: 'assets/img/200095.jpg'
  },
  {
    title: 'May',
    description: 'Arvind Bheemuck',
    rationale:'As a Flowmaster of the Hosted Platform squad, Arvind Bheemuck has been actively involved with the team on various tasks especially on MIMs and on product releases performed outside of business hours. He has also helped the squad to address blockers which would have impacted critical issue resolutions for our customers. Furthermore, Arvind has encouraged the squad to continuously improve themselves and to keep learning new technologies​',
    img: 'assets/img/200099.jpg'
  },
  {
    title: 'May',
    description: 'Keshav Ramrekha',
    rationale:'Having assumed his new role as a Platform Engineer, Keshav has consistently been solving complex issues by going out of his way to improve the quality of work for the team and helping the squad be more efficient. His hard work is truly appreciated by the team as he is always readily available to help regarding any platform issues and to provide on-going training for the new joiners of the team.​',
    img: 'assets/img/201247.jpg'
  },
  {
    title: 'July',
    description: 'Adarsh Kariman',
    rationale:'Adarsh has demonstrated hard work and loads of determination on Whitbread AHP Issue. There was an issue on average holiday calculations on live which impacted around 6000 employees. This issue had very high visibility at Excom level and would have been a major escalation for SD Worx as it directly impacted employee pay. He has been working out of office hours and on weekend to ensure the holiday pay is corrected and recalculated before the June live run. He has demonstrated resilience by putting additional effort on his end and dealt with queries in a very knowledgeable way.​',
    img: 'assets/img/500227.jpg'
  },
  {
    title: 'July',
    description: 'Ashwin Mogun',
    rationale:'Ashwin showcase a lot of dedication, great knowledge and skills as well as the endless hours of determination to resolve complex issues for our UK corporate customers. On top of his normal daily activities, Ashwin recently build an automated utility which check for errors and anomalies in costing and GL for our customers. This helped both the operations and the application support teams to identify any errors before the customer report the issue and hence has ultimately increase the confidence for the customer in proactively resolving any error on their GL.​',
    img: 'assets/img/200860.jpg'
  },
  {
    title: 'August',
    description: 'Sonakshee Chummun',
    rationale:'In Application Support, August has had its share of Major Incidents (MIM) for Whitbread (WB). Sonakshee went the extra mile, working extra hours on the weekend out of her personal time to investigate the issue and find ways to reduce the impact for the affected WB employees and the SD Worx Team. Sonakshee’s commitment and can-do attitude to have a comprehensive understanding of the issue and to be able to provide the correct elements to her stakeholders, despite the complexity of the problem was greatly appreciated by the SD Worx Operations Manager, Isla Graham and by her peers. This allowed the WB team to better understand what was happening and set the correct expectations with the client. Another MIM related to GL went on until 23:00 during the week, where Sonakshee also provided the coverage to ensure the issue is addressed on time working collaboratively with her Tier-3 colleagues. Sonakshee’s contribution to the Whitbread MIMs is highly appreciated by her peers, her Team Leader and also by the MIM Manager. ​',
    img: 'assets/img/500150.jpg'
  },
  {
    title: 'August',
    description: 'Mayanitee Ramdhany',
    rationale:'Maya is always flexible to meet customers requirements and to manage escalations. She did long hours and weekends to make sure AGS completes their UAT on time using Toolkit. Maya is very passionate about her job, and very appreciated by the UK colleagues.​',
    img: 'assets/img/200015.jpg'
  },
  {
    title: 'September',
    description: 'Premveer Gandooah',
    rationale:'Premveer has been very supportive in the past few events organized for Ops Department both in terms of logistics and ensuring that everything happen in good condition and help for cleaning up after the event. He is very active, facilitating Fun@Worx activities to ensure that we "colleagues" have the fun at Worx.​',
    img: 'assets/img/500170.jpg'
  },
  {
    title: 'October',
    description: 'Nitish Sujeebun',
    rationale:'Nitish has being of great help to ramp-up the Java resource for Globepayroll and stabilising the process on a technical ground, even being far from his core comfort zone. ​He referred and help for the technical interview of more than 30 candidates for the various areas. ​He also helped in the onbording of the interns and yound graduates while working on the internal projects for the Innovation Hub.   ​',
    img: 'assets/img/201194.jpg'
  },
  {
    title: 'October',
    description: 'Mitresh Nowrung',
    rationale:'Great Feedback from M&S due to quick turnaround at the last-minute request from M&S. Following the restructure  which is a high-level focus, Mitresh has been an incredible problem solver for M&S in gathering accurate pay data crucial to be processed within tight deadline. This was not part of the BAU activity and failure in getting those information would have a major impact for M&S.​ Mitresh has been passionate and was able to deliver at the complex request within tight deadline which made the customer really very pleased.​',
    img: 'assets/img/200660.jpg'
  },
  {
    title: 'October',
    description: 'Linda Lam',
    rationale: 'Rationale not available',
    img: 'assets/img/300537.jpg'
  },
  {
    title: 'November',
    description: 'Koossoom Seebhinijun',
    rationale:'Koossoom is very dedicated to her process and team. She has continuously provided full support to the team since July 2020 in absence of Lead. She has been supporting M&S a lot during Furlough (ongoing), taking full ownership of this process by setting up meeting with customer, providing solution of the process and also in terms of planning odd hours, delivering training to team members to help M&S for completing 100+ recalculations over the past 2 months. Koossoom has been helping M&S, Redundancy team on sensitive cases for which she has received a very good email of appreciation from M&S. Moreover, she has been helping TL with the reports for 4 consecutive months (Accuracy/Out of SLA/Calls QA) in a very timely manner.​',
    img: 'assets/img/201174.jpg'
  },
  {
    title: 'November',
    description: 'Prema Gujadhur',
    rationale:'Prema, our slide master, is always here to help or guide anybody whenever they have a query or an issue. She is also a great support to the facilities team as well as the HR team.  She always has a can-do attitude and will never refuse to take on a task, big or small.​' ,
    img: 'assets/img/500044.jpg'
  },
];
acard = [
  {
    title: 'January',
    description: 'Cynthia Daby',
    rationale: 'Rationale not available',
    img: 'assets/img/200489.jpg'
  },
  {
    title: 'February',
    description: 'Keshav Ramrekha',
    rationale: 'Rationale not available',
    img: 'assets/img/201247.jpg'
  },
  {
    title: 'February',
    description: 'Trianka Hurhinidee',
    rationale: 'Rationale not available',
    img: 'assets/img/500082.jpg'
  },
  {
    title: 'March',
    description: 'Ajay Muthoora',
    rationale: 'Rationale not available',
    img: 'assets/img/500183.jpg'
  },
  {
    title: 'April',
    description: 'Benazir Rumjan',
    rationale: 'Rationale not available',
    img: 'assets/img/500026.jpg'
  },
  {
    title: 'May',
    description: 'Sailesh Gunputh',
    rationale: 'Rationale not available',
    img: 'assets/img/500072.jpg'
  },
  {
    title: 'May',
    description: 'Yatranand Gunnoo',
    rationale: 'Rationale not available',
    img: 'assets/img/200805.jpg'
  },
  {
    title: 'June',
    description: 'Vinessen Goindananapa',
    rationale: 'Rationale not available',
    img: 'assets/img/500126.jpg'
  },
  {
    title: 'July',
    description: 'Viswamithe Ramessur',
    rationale: 'Rationale not available',
    img: 'assets/img/201113.jpg'
  },
  {
    title: 'August',
    description: 'Keshee Sowaruth',
    rationale: 'Rationale not available',
    img: 'assets/img/201593.jpg'
  },
  {
    title: 'August',
    description: 'Shabnam Rostom',
    rationale: 'Rationale not available',
    img: 'assets/img/300505.jpg'
  },
  {
    title: 'October',
    description: 'Anusha Bhujun',
    rationale: 'Rationale not available',
    img: 'assets/img/201041.jpg'
  },
  {
    title: 'October',
    description: 'Vidoushi Madhoo',
    rationale: 'Rationale not available',
    img: 'assets/img/200920.jpg'
  },
  {
    title: 'November',
    description: 'Annick Lim Hoyee',
    rationale: 'Rationale not available',
    img: 'assets/img/200220.jpg'
  },
  {
    title: 'November',
    description: 'Kevish Hurdowar',
    rationale: 'Rationale not available',
    img: 'assets/img/200636.jpg'
  },
];
slides: any = [[]];
aslide: any= [[]];
bslide: any=[[]];
chunk(arr: any, chunkSize: any) {
  let R = [];
  for (let i = 0, len = arr.length; i < len; i += chunkSize) {
    R.push(arr.slice(i, i + chunkSize));
  }
  return R;
}

}



