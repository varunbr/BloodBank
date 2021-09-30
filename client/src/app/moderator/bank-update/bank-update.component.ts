import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Bank } from 'src/app/_modals/bank';
import { User } from 'src/app/_modals/user';
import { AccountService } from 'src/app/_services/account.service';
import { ModerateService } from 'src/app/_services/moderate.service';

@Component({
  selector: 'app-bank-update',
  templateUrl: './bank-update.component.html',
  styleUrls: ['./bank-update.component.css'],
})
export class BankUpdateComponent implements OnInit {
  bank: Bank;
  currentUser: User;

  constructor(
    private route: ActivatedRoute,
    private moderateService: ModerateService,
    private toastr: ToastrService,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.accountService.user$
      .pipe(take(1))
      .subscribe((user) => (this.currentUser = user));
    this.route.params.subscribe((params) => {
      this.getBank(params['id']);
    });
  }

  getBank(id: number) {
    this.moderateService.getBank(+id).subscribe((response) => {
      this.bank = JSON.parse(JSON.stringify(response));
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

  updateBlood(ngForm: NgForm) {
    this.moderateService
      .updateBloodData({ bankId: this.bank.id, groups: this.bank.bloodGroups })
      .subscribe((response) => {
        this.bank = JSON.parse(JSON.stringify(response));
        ngForm.reset();
        this.toastr.success('Blood Data Updated');
      });
  }

  updateRoles(ngForm: NgForm) {
    this.moderateService
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

  handleFileInput(files: FileList) {
    if (files.length > 0) {
      this.moderateService
        .changePhoto(files.item(0), this.bank)
        .subscribe((response) => {
          this.bank.photoUrl = response.photoUrl;
          this.toastr.success('Photo updated.');
        });
    }
  }

  removePhoto() {
    if (!this.bank.photoUrl) {
      this.toastr.info('Bank photo already removed.');
      return;
    }
    this.moderateService.removePhoto(this.bank).subscribe(() => {
      this.bank.photoUrl = null;
      this.toastr.success('Photo removed.');
    });
  }
}
