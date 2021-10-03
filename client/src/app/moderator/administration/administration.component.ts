import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Role, RoleParams } from 'src/app/_modals/admin';
import { User } from 'src/app/_modals/user';
import { AccountService } from 'src/app/_services/account.service';
import { AdministratorService } from 'src/app/_services/administrator.service';
import { BaseListComponent } from 'src/app/_services/base-list.component';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css'],
})
export class AdministrationComponent
  extends BaseListComponent<Role, RoleParams>
  implements OnInit
{
  currentUser: User;
  adminRole: Role;
  modalRef?: BsModalRef;
  constructor(
    private administrator: AdministratorService,
    private bsmodalService: BsModalService,
    private toastr: ToastrService,
    private accountService: AccountService
  ) {
    super(administrator);
  }

  ngOnInit(): void {
    this.loadModals();
    this.accountService.user$
      .pipe(take(1))
      .subscribe((user) => (this.currentUser = user));
  }

  removeRole(adminRole: Role, index: number) {
    this.administrator.removeRole(adminRole).subscribe((response) => {
      this.modals.splice(index, 1);
      this.toastr.success('Role removed.');
    });
  }

  openModal(template: TemplateRef<any>) {
    this.adminRole = new Role('Moderator');
    this.modalRef = this.bsmodalService.show(template);
  }

  addRole() {
    this.administrator.addRole(this.adminRole).subscribe((response) => {
      this.modals.push(response);
      this.toastr.success('Role added.');
      this.modalRef.hide();
    });
  }
}
