import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificationService } from 'app/notification.service';
import { AuthService } from 'app/guards/auth.service';
import { ChangePassword } from 'app/model/change-password';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  date : Date = new Date();

  form: FormGroup;
  formUsername: string;
  formOldPassword: string;
  formNewPassword: string;
  formConfirmNewPassword: string;

  changePasswordForm: ChangePassword = new ChangePassword();

  constructor(private formBuilder: FormBuilder, private router: Router, 
    private notifyService: NotificationService, public authService: AuthService) 
  {
    this.formUsername = 'username';
    this.formOldPassword = 'oldPassword';
    this.formNewPassword = 'newPassword';
    this.formConfirmNewPassword = 'confirmNewPassword';

    this.form = this.formBuilder.group(
      {
        username: ['', [Validators.required]],
        oldPassword: ['', [Validators.required]],
        newPassword: ['', [Validators.required]],
        confirmNewPassword: ['', [Validators.required]]
      }
    )
  }

  ngOnInit(): void {
  }

  // Getters
  get username() { return this.form.get(this.formUsername); } 
  get oldPassword() { return this.form.get(this.formOldPassword); } 
  get newPassword() { return this.form.get(this.formNewPassword); } 
  get confirmNewPassword() { return this.form.get(this.formConfirmNewPassword); } 

  changePassword() {  

    if (!this.form.valid) {
      this.notifyService.showError("All fields are required! Please enter the correct details!", "");
      return;
    }

    this.changePasswordForm.username = this.form.get(this.formUsername).value;
    this.changePasswordForm.oldPassword = this.form.get(this.formOldPassword).value;
    this.changePasswordForm.newPassword = this.form.get(this.formNewPassword).value;
    this.changePasswordForm.confirmNewPassword = this.form.get(this.formConfirmNewPassword).value;

    if(!(this.changePasswordForm.newPassword == this.changePasswordForm.confirmNewPassword))
    {
      this.notifyService.showError("Passwords do not match!", "");
    }
    else
    {
      this.authService.changePassword(this.changePasswordForm)
      .subscribe((data) => {

        console.log(data);

        if(data == false)
        {
          this.notifyService.showError("Invalid data! Please enter the correct details!", "");
        }

        if(data == true)
        {
          this.notifyService.showSuccess("Password updated successfully !!", "");

          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 5000);  // 5 sec
        }

      });        
    }
  }

}
