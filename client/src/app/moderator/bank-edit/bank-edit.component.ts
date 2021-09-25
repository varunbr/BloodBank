import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Bank } from 'src/app/_modals/bank';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-bank-edit',
  templateUrl: './bank-edit.component.html',
  styleUrls: ['./bank-edit.component.css'],
})
export class BankEditComponent implements OnInit {
  bank: Bank;
  bankUpdateForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private adminService: AdminService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.getBank(params['id']);
    });
  }

  initilizeForm() {
    this.bankUpdateForm = this.fb.group({
      id: [this.bank.id],
      name: [this.bank.name, Validators.required],
      website: [this.bank.website],
      phoneNumber: [this.bank.phoneNumber, Validators.required],
      email: [this.bank.email, [Validators.email, Validators.required]],
      area: [this.bank.area, Validators.required],
      city: [this.bank.city, Validators.required],
      state: [this.bank.state, Validators.required],
      country: [this.bank.country, Validators.required],
      postalCode: [this.bank.postalCode, Validators.required],
    });
  }

  getBank(id: number) {
    this.adminService.getBank(+id).subscribe((response) => {
      this.bank = JSON.parse(JSON.stringify(response));
      this.initilizeForm();
    });
  }

  addRole(ngForm: NgForm) {
    this.bank.moderators.push({
      type: 'BankModerator',
      userId: 0,
      userName: '',
    });
    ngForm.form.markAsDirty();
  }

  removeRole(ngForm: NgForm, index: number) {
    this.bank.moderators.splice(index, 1);
    ngForm.form.markAsDirty();
  }

  updateBank() {
    this.adminService
      .updateBank(this.bankUpdateForm.value)
      .subscribe((response) => {
        this.bank = JSON.parse(JSON.stringify(response));
        this.initilizeForm();
        this.toastr.success('Bank Updated');
      });
  }

  updateRoles(ngForm: NgForm) {
    this.adminService
      .updateBankRoles({
        bankId: this.bank.id,
        moderators: this.bank.moderators,
      })
      .subscribe((response) => {
        this.bank = JSON.parse(JSON.stringify(response));
        ngForm.reset();
        this.toastr.success('Roles Updated');
      });
  }
}
