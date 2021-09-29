import {
  Directive,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { User } from '../_modals/user';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]',
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[];
  user: User;
  constructor(
    private templateRef: TemplateRef<any>,
    private vcRef: ViewContainerRef,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.accountService.user$.subscribe((user) => {
      this.user = user;
      this.createView();
    });
  }

  createView() {
    if (this.user == null || !this.user.roles) {
      this.vcRef.clear();
    } else if (this.user.roles.some((r) => this.appHasRole.includes(r))) {
      this.vcRef.createEmbeddedView(this.templateRef);
    } else {
      this.vcRef.clear();
    }
  }
}
