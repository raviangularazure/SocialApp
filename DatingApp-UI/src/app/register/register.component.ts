import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  user: User;
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>; // The Partial class makes ALL of the properties inside the type as optional. 

  constructor(private authService: AuthService, private alertify: AlertifyService, private fb: FormBuilder,
    private router: Router) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    };

    this.createRegisterForm();

  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordsMatchValidator });
  }

  passwordsMatchValidator(group: FormGroup) {
    return group.get('password').value == group.get('confirmPassword').value ? null : { 'mismatch': true };
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);

      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('Registration successful');
      }, error => this.alertify.error(error),
        () => {
          this.authService.login(this.user).subscribe(() => {
            this.router.navigate(['/members']);
          })
        });
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
