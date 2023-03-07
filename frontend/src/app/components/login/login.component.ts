import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

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

  constructor(private formBuilder: FormBuilder) { }

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

  onSubmit(){
    if(this.loginForm.valid){
      console.log(this.loginForm.value);
      //Send the object to database
    }else{
      console.log('Form is not valid');
      //throw the error using toaster and with required fields

      this.validateAllFormFields(this.loginForm);
      alert("Your form is invalid");
    }
  }

  private validateAllFormFields(formGroup: FormGroup){
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);

      if(control instanceof FormControl){
        control.markAsDirty({onlySelf: true});
        
      }else if(control instanceof FormGroup){
        this.validateAllFormFields(control);
      }
    })
  }

}
