import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-bank-register',
  templateUrl: './bank-register.component.html',
  styleUrls: ['./bank-register.component.css'],
})
export class BankRegisterComponent implements OnInit {
  bankRegisterForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initilizeForm();
  }

  initilizeForm() {
    this.bankRegisterForm = this.fb.group({
      name: ['', Validators.required],
      website: [''],
      phoneNumber: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      area: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      country: ['', Validators.required],
      postalCode: ['', Validators.required],
      bankAdmin: ['', Validators.required],
    });
  }

  registerBank() {
    this.adminService
      .registerBank(this.bankRegisterForm.value)
      .subscribe((response) => {
        this.toastr
          .success('Bank registered, BankId - ' + response)
          .onTap.pipe(take(1))
          .subscribe(() => {
            this.router.navigateByUrl('/admin/' + response);
          });
        this.bankRegisterForm.reset();
      });
  }
}
