import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import ValidateForm from 'src/app/helpers/validateform';
import { Login } from 'src/app/shared/login.model';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  loginForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, 
    private _authService: AuthService, 
    private _router: Router) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? ((this.eyeIcon = "fa-eye") && (this.type = "text")) : 
        ((this.eyeIcon = "fa-eye-slash") && (this.type = "password"))
  }

  async onLogin(){
    if(this.loginForm.valid){
      console.log(this.loginForm.value);
      await this._authService.login(this.loginForm.value as Login).subscribe({
        next: () => {
          this.loginForm.reset();
          this._router.navigate(['sales']);
        },
        error: (err) => {
          alert(err?.error.message);
        }
      });
    }else{
      console.log('Form is not valid');
      //throw the error using toaster and with required fields

      ValidateForm.validateAllFormFields(this.loginForm);
      alert("Your form is invalid");
    }
  }
}
