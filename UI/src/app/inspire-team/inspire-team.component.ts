import { Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, AbstractControl, FormArray } from '@angular/forms';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { ApiService } from 'app/api.service';
import { NotificationService } from 'app/notification.service';
import { FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { DatePipe } from '@angular/common';
import { InspireTeam } from 'app/model/inspire-team';

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-inspire-team',
  templateUrl: './inspire-team.component.html',
  styleUrls: ['./inspire-team.component.css'],
  providers: [{ provide: STEPPER_GLOBAL_OPTIONS, useValue: { displayDefaultIndicatorType: false }}]
})

export class InspireTeamComponent implements OnInit {

  [x: string]: any;
  isLinear = true;
  title = 'toaster-not';
  errorMessage: string;
  
  voterPayrollId;

  nominationPeriod;

  teamNameFourthStepper: string;
  
  // for first stepper
  formTeamName: string;

  teamName = new FormControl('', Validators.required);
  
  // for second stepper
  formQuality: string;
  formSatisfaction: string;
  formBranding: string;
  formBenefits: string;
  formOther: string;

  formTextareaQuality: string;
  formTextareaSatisfaction: string;
  formTextareaBranding: string;
  formTextareaFinancial: string;
  formTextareaOther: string;

  // for third stepper
  formCommitment: string;
  formBelieve: string;
  formRoadOpen: string;
  formOneForAll: string;

  formTextareaCommitment: string;
  formTextareaBelieve: string;
  formTextareaRoadOpen: string;
  formTextareaOneForAll: string;

  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;
  forthFormGroup: FormGroup;
  inspireTeamFinal: InspireTeam = new InspireTeam();

  //for second stepper - Impact
  arrayImpact: string[] = [];

  //for third stepper - Spark
  arraySpark: string[] = [];

  // labels for checkboxes for Impact
  Impact = [
    { label: 'Improved quality of service', checked: false, placeholder: 'How has the team improved the quality of service?'},
    { label: 'Customer satisfaction (external/internal)', checked: false, placeholder: 'How has the team lead to customer satisfaction?'},
    { label: 'SD Worx branding', checked: false, placeholder: 'How has the team contributed towards SD Worx branding?' },
    { label: 'Financial benefits', checked: false, placeholder: 'How has the team lead to financial benefits?'},
    { label: 'Other', checked: false, placeholder: 'Provide impact and rationale for nomination here'}
  ];

   // labels for checkboxes for Spark
   Spark = [
    { label: 'Dear customer', checked: false, placeholder: 'Give a brief description of the expertise showcased by the nominee', description:"We always put our best foot forward for our customers. By placing their needs and requirements in front of anything and everything else, we keep them ahead of the game. Yet, we manage expectations and are not afraid to challenge questions. We really listen and put ourselves in the shoes of the people that use our products and services. By truly understanding their goals, we use our expertise to provide the best quality service with passion and professionalism." },
    { label: 'Commitment drives us forward', checked: false, placeholder: 'Give a brief description of the commitment showcased by the team', description: "We have the will to succeed and we feel the need to go forward. We take pride and ownership in our work, from the beginning to completion. Every day, we take charge of the situation and look after our customers by guiding them in the right direction. No matter how big the challenge or how many stumbling blocks along the way, we are dedicated to deliver results. With confidence, we look ahead and keep on moving, finding the way to new solutions." },
    { label: 'We believe in each other', checked: false, placeholder: 'Give a brief description of the trust and integrity showcased by the team', description:"Trust and integrity are what we stand for. We have faith in each other to do what’s expected. Both our colleagues and our customers rely on us to look after them and take them in the right direction. We have confidence in our own abilities and feel empowered to make our own decisions, knowing that the team will always support us. Trust and honesty are closely tied together, we’re sincere and we dare to set boundaries. In other words, we stick to our promises, but we only promise what we can keep." },
    { label: 'The road is open', checked: false, placeholder: 'Give a brief description of the innovation and integrity showcased by the team', description:"We dare to look at things from a different perspective. Not always knowing what comes next, we look in all directions to form new ideas to make things better. Innovation is in our DNA and imagination is our best friend, it inspires creativity and pushes us to see possibilities beyond the realities of today. Looking into the future, we accept new challenges without prejudice. We are open to learn new things and to question old habits. This requires us to be bold and positive. We can only be truly open minded if we rely our spontaneous enthusiasm, without taking life all too seriously." },
    { label: 'One for all, all for one', checked: false, placeholder: 'Give a brief description of the togetherness showcased by the team', description:"We are close and feel interconnected, even if we’re physically apart. With the interest of the team at heart, we all pull together with a clear view on where we’re going. All team members are equal partners, and everyone knows their roles & responsibilities. Even though we share our differences, we always keep the dialogue going. With respect for different opinions, we understand that we can achieve so much more when we join forces. Working closely together triggers a sense of belonging and camaraderie: we’re all part of the SD Worx family." }
  ];

  disableButtonNext2Button = true;
  disableButtonNext3Button = true;

  constructor(private formBuilder: FormBuilder, private service: ApiService, 
    private notifyService: NotificationService, public datepipe: DatePipe) {

    this.formTeamName = 'teamName';

    this.formQuality = 'quality';
    this.formSatisfaction = 'satisfaction';
    this.formBranding = 'branding';
    this.formBenefits = 'benefits';
    this.formOther = 'other';

    this.formTextareaQuality = 'textareaQuality';
    this.formTextareaSatisfaction = 'textareaSatisfaction';
    this.formTextareaBranding = 'textareaBranding';
    this.formTextareaFinancial = 'textareaFinancial';
    this.formTextareaOther = 'textareaOther';

    this.formCommitment = 'commitment';
    this.formBelieve = 'believe';
    this.formRoadOpen = 'roadopen';
    this.formOneForAll = 'oneforall';

    this.formTextareaCommitment = 'textareaCommitment';
    this.formTextareaBelieve = 'textareaBelieve';
    this.formTextareaRoadOpen = 'textareaRoadOpen';
    this.formTextareaOneForAll = 'textareaOneForAll';    
  }

  ngOnInit(): void {

    // get current nomination period
    this.service.getPeriod()
    .subscribe((data) => {

      this.nominationPeriod = data;
    });
   
    // first stepper
    this.firstFormGroup = this.formBuilder.group(
      {
        teamName: ['', [Validators.required]]
      }
    );

    // second stepper
    this.secondFormGroup = this.formBuilder.group(
      {
        secondFormArray: this.formBuilder.array([])
      }
    );
    
    const control = <FormArray>this.secondFormGroup.controls['secondFormArray'];
    this.Impact.forEach( x => {
      control.push(this.createSecondFormGroup(x));
    })

    control.valueChanges.subscribe( x => {
      const test = x.filter( z => z.checked === true);
      if (test && test.length < 1) {
        this.disableButtonNext2Button = true;
      } else {
        this.disableButtonNext2Button = false;
      }
    })

    // third stepper
    this.thirdFormGroup = this.formBuilder.group(
      {
        thirdFormArray: this.formBuilder.array([])
      }
    );
    
    const controll = <FormArray>this.thirdFormGroup.controls['thirdFormArray'];
    this.Spark.forEach( x => {
      controll.push(this.createThirdFormGroup(x));
    })

    controll.valueChanges.subscribe( x => {
      const test = x.filter( z => z.checked === true);
      if (test && test.length < 1) {
        this.disableButtonNext3Button = true;
      } else {
        this.disableButtonNext3Button = false;
      }
    })

    this.forthFormGroup = this.formBuilder.group(
      {
        teamName: ['', [Validators.required]],
        textareaQuality: [''],
        textareaSatisfaction: [''],
        textareaBranding: [''],
        textareaFinancial: [''],
        textareaOther: [''],
        textareaCommitment: [''],
        textareaBelieve: [''],
        textareaRoadOpen: [''],
        textareaOneForAll: ['']
      })
  }

  getErrorMessage() {
    if (this.teamName.hasError('required')) {
      return 'Please enter team name';
    }
  }

  // should put these 3 empty functions else error - cannot retrieve text area values if removed
  form1() {
  }

  form2() {
  }

  form3() {
  }

  form4() {
    
  }

  getPeriod() 
  { 
    // get current nomination period
    this.service.getPeriod()
    .subscribe((data) => {

      this.nominationPeriod = data;
    }); 
    return this.nominationPeriod;
  }

  click : boolean = false;

  onButtonClick(){
    this.click = !this.click;
  }

  onKey(event: KeyboardEvent) { 
    // if value is not empty the set click to false otherwise true
    this.click = (event.target as HTMLInputElement).value === '' ? true:false;  
  }

  callService() {

    this.inspireTeamFinal.teamName = this.firstFormGroup.get(this.formTeamName).value,
 
    this.secondFormGroup.value.secondFormArray.forEach(element => {
      if(element.checked){
        this.arrayImpact.push(element.name, element.reason);
      }
    });

    this.thirdFormGroup.value.thirdFormArray.forEach(element => {
      if(element.checked){
        this.arraySpark.push(element.name, element.reason);
      }
    });

    // second stepper
    this.inspireTeamFinal.Impact = this.arrayImpact.toString();

    // third stepper
    this.inspireTeamFinal.BeASpark = this.arraySpark.toString();

    this.voterPayrollId = Number(localStorage.getItem('voterPayrollId'));
    this.inspireTeamFinal.VoterPayrollId = this.voterPayrollId; // voterid == person who logged in

    this.inspireTeamFinal.Period = this.getPeriod(); 
  
    this.inspireTeamFinal.DateCreated = new Date();

    this.inspireTeamFinal.DateLastModified = new Date();

    this.inspireTeamFinal.EventId = 3; // event = Inspire Team

    this.service.saveDetailsInspire(this.inspireTeamFinal)
      .subscribe(() => {
      });

    this.notifyService.showSuccess("Details submitted successfully !!", "");

    setTimeout(() => window.location.reload(), 500);
  }

  /* *********** Second Stepper ************ */

  visible: boolean;
  CurrentItem:number

  mouseEnter(i) {
    this.visible = true;
    
    this.CurrentItem=i;
  }

  mouseLeave(i) {
    this.visible = false;
    this.CurrentItem = null;
  }

  onChange($value, ind) {

    const control = <FormArray>this.secondFormGroup.controls['secondFormArray'];
    control.controls.find((val, index) => index === ind).get('checked').setValue($value.checked)

    if(!$value.checked) {
      control.controls.find((val, index) => index === ind).get('reason').setValue('');
      control.controls.find((val, index) => index === ind).get('reason').setValidators(null)
    } 
    else {
      control.controls.find((val, index) => index === ind).get('reason').setValidators(
        Validators.compose([Validators.required, Validators.minLength(25), Validators.maxLength(250)])   
      )
    }
    control.controls.find((val, index) => index === ind).get('reason').updateValueAndValidity({emitEvent: false});
  }

  
  /* *********** End of Second Stepper ************ */


  /* *********** Third Stepper ************ */

  onChangeSpark($value, ind){
    this.teamNameFourthStepper= this.firstFormGroup.get(this.formTeamName).value;

    const control = <FormArray>this.thirdFormGroup.controls['thirdFormArray'];
    control.controls.find((val, index) => index === ind).get('checked').setValue(
      $value.checked
    )
    
    if(!$value.checked) {
      control.controls.find((val, index) => index === ind).get('reason').setValue('');
      control.controls.find((val, index) => index === ind).get('reason').setValidators(null)
    } else {
      control.controls.find((val, index) => index === ind).get('reason').setValidators(
        Validators.compose([Validators.required, Validators.minLength(25), Validators.maxLength(250)])
      )
    }
    control.controls.find((val, index) => index === ind).get('reason').updateValueAndValidity({emitEvent: false});
  }


  /* *********** End of Third Stepper ************ */

  getSecondFormControl(): AbstractControl[] {
    return (this.secondFormGroup.get('secondFormArray') as FormArray).controls;
  }

  createSecondFormGroup(impact): FormGroup {
    return this.formBuilder.group({
      name: [impact.label],
      checked: [null],
      placeholder: [impact.placeholder],
      reason: ['']
    });
  }

  getThirdFormControl(): AbstractControl[] {
    return (this.thirdFormGroup.get('thirdFormArray') as FormArray).controls;
  }

  createThirdFormGroup(spark): FormGroup {
    return this.formBuilder.group({
      name: [spark.label],
      checked: [null],
      placeholder: [spark.placeholder],
      reason: [''],
      description: [spark.description]
    });
  }

  getSecondFormError(ind) {
    const control = <FormArray>this.secondFormGroup.controls['secondFormArray'];
    const error = control.controls.find((val, index) => index === ind).get('reason')
    if(error && error?.errors?.required){
      this.errorMessage = 'This field is required.'
      return true;
    }
    else if(error && error?.errors?.minlength){
      this.errorMessage = 'This field must contain at least 25 characters.'
      return true;
    }
    else if(error && error?.errors?.maxlength){
      this.errorMessage = 'This field cannot exceed 250 characters.'
      return true;
    }
    return false;
  }

  getThirdFormError(ind) {
    const control = <FormArray>this.thirdFormGroup.controls['thirdFormArray'];
    const error = control.controls.find((val, index) => index === ind).get('reason')
    if(error && error?.errors?.required){
      this.errorMessage = 'This field is required.'
      return true;
    }
    else if(error && error?.errors?.minlength){
      this.errorMessage = 'This field must contain at least 25 characters.'
      return true;
    }
    else if(error && error?.errors?.maxlength){
      this.errorMessage = 'This field cannot exceed 250 characters.'
      return true;
    }
    return false;
  }

}



