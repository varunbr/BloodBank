import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { validateUserExistence } from '../directives/user-name-validator.directive';
import { getBloodGroupList } from '../_modals/utility';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  validationErrors: string[] = [];
  maxDate: Date;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router
  ) {
    this.initilizeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 15);
  }

  ngOnInit(): void {}

  initilizeForm() {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      userName: [
        '',
        [Validators.required, Validators.minLength(3)],
        validateUserExistence(this.accountService, false),
      ],
      phoneNumber: ['', Validators.required],
      email: ['', Validators.email],
      dateOfBirth: ['', Validators.required],
      gender: ['Male', Validators.required],
      bloodGroup: ['', Validators.required],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(12),
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
      area: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      country: ['', Validators.required],
      postalCode: ['', Validators.required],
    });

    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  matchValues(name: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      return control?.value === control?.parent?.controls[name]?.value
        ? null
        : { notMatching: true };
    };
  }

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe(
      () => {
        this.router.navigateByUrl('/');
      },
      (error) => {
        this.validationErrors = [error.error];
        console.log(error);
      }
    );
  }

  bloodGroupList = getBloodGroupList();
}
