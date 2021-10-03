import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Role } from 'src/app/_modals/admin';
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
  bankModerator: Role;
  modalRef?: BsModalRef;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private adminService: AdminService,
    private toastr: ToastrService,
    private bsmodalService: BsModalService
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

  openModal(template: TemplateRef<any>) {
    this.bankModerator = new Role('BankModerator');
    this.modalRef = this.bsmodalService.show(template);
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

  addRole() {
    this.adminService
      .addBankRole(this.bankModerator, this.bank.id)
      .subscribe((response) => {
        this.bank = JSON.parse(JSON.stringify(response));
        this.toastr.success('Role Added');
        this.modalRef.hide();
      });
  }

  removeRole(role: Role) {
    this.adminService
      .removeBankRole(role, this.bank.id)
      .subscribe((response) => {
        this.bank = JSON.parse(JSON.stringify(response));
        this.toastr.success('Role removed');
      });
  }
}
