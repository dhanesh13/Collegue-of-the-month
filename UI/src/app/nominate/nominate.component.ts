import { Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, AbstractControl, FormArray, MinLengthValidator } from '@angular/forms';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { ApiService } from 'app/api.service';
import { Employee } from 'app/model/employee-model';
import { HttpClient } from '@angular/common/http';
import { NotificationService } from 'app/notification.service';
import { VotingForm } from 'app/model/voter-form-model';
import { MatSelectChange } from '@angular/material/select';
import { map, startWith } from 'rxjs/operators';
import { FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { DatePipe } from '@angular/common';
import { RoleService } from 'app/guards/role.service';

/** Error when invalid control is dirty, touched, or submitted. For dropdown */
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-nominate',
  templateUrl: './nominate.component.html',
  styleUrls: ['./nominate.component.css'],
  providers: [{
    provide: STEPPER_GLOBAL_OPTIONS, useValue: { displayDefaultIndicatorType: false }
  }]
})

export class NominateComponent implements OnInit {
  filteredOptions;
  [x: string]: any;
  isLinear = true;
  public employees: Employee[];
  isHidden= true;
  title = 'toaster-not';
  errorMessage: string;
  // for fields not displayed in html page
  nomPayrollId;
  voterPayrollId;
  period;

  nominationPeriod;

  nomineeFullName: string;
  
  // for first stepper
  formFullName: string;
  formDivision: string;
  formDepartment: string;
  formManager: string;

  fullName = new FormControl('', Validators.required);
  
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
  votingFormFinal: VotingForm = new VotingForm();

  //for second stepper - Impact
  arrayImpact: string[] = [];

  //for third stepper - Spark
  arraySpark: string[] = [];

  // labels for checkboxes for Impact
  Impact = [
    { label: 'Improved quality of service', checked: false, placeholder: 'How has the nominee improved the quality of service?'},
    { label: 'Customer satisfaction (external/internal)', checked: false, placeholder: 'How has the nominee lead to customer satisfaction?'},
    { label: 'SD Worx branding', checked: false, placeholder: 'How has the nominee contributed towards SD Worx branding?' },
    { label: 'Financial benefits', checked: false, placeholder: 'How has the nominee lead to financial benefits?'},
    { label: 'Other', checked: false, placeholder: 'Provide impact and rationale for nomination here'}
  ];

   // labels for checkboxes for Spark
   Spark = [
    { label: 'Dear customer', checked: false, placeholder: 'Give a brief description of the expertise showcased by the nominee', description:"We always put our best foot forward for our customers. By placing their needs and requirements in front of anything and everything else, we keep them ahead of the game. Yet, we manage expectations and are not afraid to challenge questions. We really listen and put ourselves in the shoes of the people that use our products and services. By truly understanding their goals, we use our expertise to provide the best quality service with passion and professionalism." },
    { label: 'Commitment drives us forward', checked: false, placeholder: 'Give a brief description of the commitment showcased by the nominee', description: "We have the will to succeed and we feel the need to go forward. We take pride and ownership in our work, from the beginning to completion. Every day, we take charge of the situation and look after our customers by guiding them in the right direction. No matter how big the challenge or how many stumbling blocks along the way, we are dedicated to deliver results. With confidence, we look ahead and keep on moving, finding the way to new solutions." },
    { label: 'We believe in each other', checked: false, placeholder: 'Give a brief description of the trust and integrity showcased by the nominee', description:"Trust and integrity are what we stand for. We have faith in each other to do what’s expected. Both our colleagues and our customers rely on us to look after them and take them in the right direction. We have confidence in our own abilities and feel empowered to make our own decisions, knowing that the team will always support us. Trust and honesty are closely tied together, we’re sincere and we dare to set boundaries. In other words, we stick to our promises, but we only promise what we can keep." },
    { label: 'The road is open', checked: false, placeholder: 'Give a brief description of the innovation and integrity showcased by the nominee', description:"We dare to look at things from a different perspective. Not always knowing what comes next, we look in all directions to form new ideas to make things better. Innovation is in our DNA and imagination is our best friend, it inspires creativity and pushes us to see possibilities beyond the realities of today. Looking into the future, we accept new challenges without prejudice. We are open to learn new things and to question old habits. This requires us to be bold and positive. We can only be truly open minded if we rely our spontaneous enthusiasm, without taking life all too seriously." },
    { label: 'One for all, all for one', checked: false, placeholder: 'Give a brief description of the togetherness showcased by the nominee', description:"We are close and feel interconnected, even if we’re physically apart. With the interest of the team at heart, we all pull together with a clear view on where we’re going. All team members are equal partners, and everyone knows their roles & responsibilities. Even though we share our differences, we always keep the dialogue going. With respect for different opinions, we understand that we can achieve so much more when we join forces. Working closely together triggers a sense of belonging and camaraderie: we’re all part of the SD Worx family." }
  ];

  disableButtonNext2Button = true;
  disableButtonNext3Button = true;

  constructor(private formBuilder: FormBuilder, private service: ApiService, private http: HttpClient, 
    private notifyService: NotificationService, public datepipe: DatePipe, private roleService: RoleService) {
    this.formFullName = 'fullName';
    this.formDivision = 'division';
    this.formDepartment = 'department';
    this.formManager = 'manager';

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

    this.EmpList();

    // get current nomination period
    this.service.getPeriod()
    .subscribe((data) => {

      this.nominationPeriod = data;
    });
   
    this.firstFormGroup = this.formBuilder.group(
      {
        fullName: ['', [Validators.required]],
        division: ['', [Validators.required]],
        department: ['', [Validators.required]],
        manager: ['', [Validators.required]],
        period: ['']
      }
    );
  
    this.firstFormGroup.get('fullName').valueChanges
      .subscribe(data => {
        // if (data) {          
        //   this.nomPayrollId =  data.payrollID;        
        //   this.firstFormGroup.get('division').setValue(data.employeeDivision),
        //     this.firstFormGroup.get('department').setValue(data.employeeDepartment),
        //     this.firstFormGroup.get('manager').setValue(data.employeeFirstNameManager + ' '+ data.employeeLastNameManager)
        // }
        
      });
      
    // for select fullname
    this.firstFormGroup.get('fullName').valueChanges.subscribe(
      value => {
        if(value){
          this.isHidden = false;
        }
      }
    )

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
        fullName: ['', [Validators.required]],
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

  selectedEmployee(data){

    this.nomPayrollId =  data.payrollID;        
    this.firstFormGroup.get('division').setValue(data.employeeDivision);
    this.firstFormGroup.get('department').setValue(data.employeeDepartment);
    this.firstFormGroup.get('manager').setValue(data.employeeFirstNameManager + ' '+ data.employeeLastNameManager);

    this.isHidden = false;
  }

  getNomPeriod() {
    var months = new Array("January", "February", "March", "April", "May", "June",
      "July", "August", "September", "October", "November", "December");

    var todayDate = new Date();
    var monthName = months[todayDate.getMonth()];
    var year = todayDate.getFullYear();
    this.nominationPeriod = monthName + " " + year;
    return this.nominationPeriod;
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

  selectedFullName: string;

  selectedValue(event: MatSelectChange) {
    this.selectedData = {
      text: event.source.triggerValue
    };
    this.selectedFullName = this.selectedData.text
  }

  // Impact
  isHiddenQuality = true;
  isHiddenSatisfaction = true;
  isHiddenBranding = true;
  isHiddenFinancial = true;
  isHiddenOther = true;

  // Spark
  isHiddenCommitment = true;
  isHiddenBelieve = true;
  isHiddenRoadOpen = true;
  isHiddenOneForAll = true;

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

    this.votingFormFinal.NomineePayrollId = this.nomPayrollId;

    // second stepper
    this.votingFormFinal.Impact = this.arrayImpact.toString();

    // third stepper
    this.votingFormFinal.BeASpark = this.arraySpark.toString();

    this.voterPayrollId = Number(localStorage.getItem('voterPayrollId'));
    this.votingFormFinal.VoterPayrollId = this.voterPayrollId; // voterid == person who logged in

    this.votingFormFinal.Period = this.getPeriod(); 
  
    this.votingFormFinal.DateCreated = new Date();

    this.votingFormFinal.DateLastModified = new Date();

    this.votingFormFinal.eventId = 1; // event = COTM

    // person who logged in cannot vote for himself
    if(this.votingFormFinal.NomineePayrollId == this.votingFormFinal.VoterPayrollId)
    {
      this.notifyService.showError("You cannot vote for yourself !!", "");
      setTimeout(() => window.location.reload(), 2000);
    }
    else
    {
      this.service.saveDetails(this.votingFormFinal)
      .subscribe((data) => {

        if(data == false)
        {
          this.notifyService.showSuccess("Details submitted successfully !!", "");
          setTimeout(() => window.location.reload(), 500);
        }
        if(data == true)
        {
          this.notifyService.showError("You have already voted for this person !!", "");
          setTimeout(() => window.location.reload(), 2000);
        }
      });
    }

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
    this.nomineeFullName= this.firstFormGroup.get(this.formFullName).value.employeeFirstName + ' ' + this.firstFormGroup.get(this.formFullName).value.employeeLastName;
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
    this.nomineeFullName= this.firstFormGroup.get(this.formFullName).value.employeeFirstName + ' ' + this.firstFormGroup.get(this.formFullName).value.employeeLastName;
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


  EmpList() {

    this.service.getEmpDetails().subscribe((data: Employee[]) => {
      this.employees = data;
      this.filteredOptions = this.firstFormGroup.get('fullName').valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filter(value): this.employees)
      );

    }), (error: any) => { console.log(error); }
  }

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

  displayFullname(employee) {
    if(employee) {
      return (employee.employeeFirstName + ' ' + employee.employeeLastName)
    }
  }

  clearFullName(){
    this.firstFormGroup.reset();

    this.isHidden = true;
  }

  private _filter(value: string): any {
    if(typeof(value) === 'string') {
      const filterValue = value ? value.toLowerCase() : '';
      let emp: Employee[] = [];
      if(this.employees) {
        this.employees.map(option =>{
          if( (option.employeeFirstName.toLowerCase()).includes(filterValue) || 
              (option.employeeLastName.toLowerCase()).includes(filterValue)) {
            emp.push(option)
          }
          }
        );
        return emp
      }
    }
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

