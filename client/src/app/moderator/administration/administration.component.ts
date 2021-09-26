import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { AdminRole, AdminRoleParams } from 'src/app/_modals/admin';
import { AdministratorService } from 'src/app/_services/administrator.service';
import { BaseListComponent } from 'src/app/_services/base-list.component';

@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css'],
})
export class AdministrationComponent
  extends BaseListComponent<AdminRole, AdminRoleParams>
  implements OnInit
{
  adminRole: AdminRole;
  modalRef?: BsModalRef;
  constructor(
    private administrator: AdministratorService,
    private bsmodalService: BsModalService,
    private toastr: ToastrService
  ) {
    super(administrator);
  }

  ngOnInit(): void {
    this.loadModals();
  }

  removeRole(adminRole: AdminRole, index: number) {
    this.administrator.removeRole(adminRole).subscribe((response) => {
      this.modals.splice(index, 1);
      this.toastr.success('Role removed.');
    });
  }

  openModal(template: TemplateRef<any>) {
    this.adminRole = new AdminRole();
    this.modalRef = this.bsmodalService.show(template);
  }

  addRole() {
    console.log('Add');
    this.administrator.addRole(this.adminRole).subscribe((response) => {
      this.modals.push(response);
      this.toastr.success('Role added.');
      this.modalRef.hide();
    });
  }
}
