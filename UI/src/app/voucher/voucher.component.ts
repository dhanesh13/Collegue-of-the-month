import { Component, OnInit } from '@angular/core';
import { ApiService } from 'app/api.service';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { NotificationService } from 'app/notification.service';
import { Voucher } from 'app/model/voucher';

@Component({
  selector: 'app-voucher',
  templateUrl: './voucher.component.html',
  styleUrls: ['./voucher.component.css']
})
export class VoucherComponent implements OnInit {

  nominationPeriod;

  form: FormGroup;
  formVoucher: string;

  isSubmitted = false;

  nomineeId: number;

  firstName: string;
  lastName: string;
  firstNameManager: string;
  lastNameManager: string;

  voucherFinal: Voucher = new Voucher();

  constructor(private apiService: ApiService, private formBuilder: FormBuilder, private notifyService: NotificationService) {
    this.formVoucher = 'voucher';

    this.form = this.formBuilder.group(
      {   
        voucher: ['', [Validators.required]]
      }
    )
  }

  ngOnInit(): void {

    // get current nomination period
    this.apiService.getPeriod()
    .subscribe((data) => {

      this.nominationPeriod = data;
    });
  }

  // getDetails(){
  //   this.nomineeId = Number(localStorage.getItem('voterPayrollId'));

  //   this.apiService.getShortlistedNomineesDetailsByNomineePayrollID(this.nomineeId.toString())
  //   .subscribe((response) => {

  //   }); 
  // }

  save() {
    this.isSubmitted = true;

    if (!this.form.valid) {
      return;
    }
    else
    {
      // this.getDetails();
     // console.log("submitted successfully");

      this.voucherFinal.VoucherName = this.form.get(this.formVoucher).value;

      this.nomineeId = Number(localStorage.getItem('voterPayrollId'));
      this.voucherFinal.PayrollID = this.nomineeId;

      this.voucherFinal.EventID = 1; // event = COTM

      this.voucherFinal.Period = this.nominationPeriod;

      this.apiService.saveDetailsVoucher(this.voucherFinal)
      .subscribe((data) => {
        console.log(data);

        if(data == true)
        {
          this.notifyService.showSuccess("Submission successful", "");
          setTimeout(() => window.location.reload(), 3000);
        }
        if(data == false)
        {
          this.notifyService.showError("You have already submitted your voucher !!", "");
          setTimeout(() => window.location.reload(), 2000);
        }
     
      });

      
    }  
  }

  get voucher() { return this.form.get(this.formVoucher); } 

}
