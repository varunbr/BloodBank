import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { validateUserExistence } from '../directives/user-name-validator.directive';
import { UserProfile } from '../_modals/profile';
import { getBloodGroupList } from '../_modals/utility';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css'],
})
export class EditProfileComponent implements OnInit {
  user: UserProfile;
  profileUpdateForm: FormGroup;
  validationErrors: string[] = [];
  bloodGroupList = getBloodGroupList();
  maxDate: Date;
  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private toastr: ToastrService
  ) {
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  ngOnInit(): void {
    this.accountService.getProfile().subscribe((response) => {
      this.user = response;
      this.initilizeForm();
    });
  }

  initilizeForm() {
    this.profileUpdateForm = this.fb.group({
      id: [this.user.id],
      name: [this.user.name, Validators.required],
      userName: [
        this.user.userName,
        [Validators.required, Validators.minLength(3)],
        validateUserExistence(this.accountService, false, this.user.userName),
      ],
      bloodGroup: [this.user.bloodGroup, Validators.required],
      dateOfBirth: [new Date(this.user.dateOfBirth), Validators.required],
      phoneNumber: [this.user.phoneNumber, Validators.required],
      email: [this.user.email, [Validators.email, Validators.required]],
      area: [this.user.area, Validators.required],
      city: [this.user.city, Validators.required],
      state: [this.user.state, Validators.required],
      country: [this.user.country, Validators.required],
      postalCode: [this.user.postalCode, Validators.required],
      gender: [this.user.gender, Validators.required],
    });
  }

  updateProfile() {
    this.accountService
      .updateProfile(this.profileUpdateForm.value)
      .subscribe((response) => {
        this.user = response;
        this.initilizeForm();
        this.toastr.success('Profile updated.');
      });
  }
}
