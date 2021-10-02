
import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { RationaleService } from 'app/rationale.service';

@Component({
  selector: 'app-rationale-spark',
  templateUrl: './rationale-spark.component.html',
  styleUrls: ['./rationale-spark.component.css']
})

export class RationaleSparkComponent implements OnInit {
  
  receivedData: any;
  receivedData$: any;

  constructor(private rationaleService:RationaleService, public activeModal: NgbActiveModal) { }

  ngOnInit(): void {
    
    this.rationaleService.data.subscribe(
      response => {
        this.receivedData = response;
    });

    this.receivedData$= this.receivedData.nominations;
  }
}
