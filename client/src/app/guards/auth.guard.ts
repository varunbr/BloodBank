import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private route: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.accountService.user$.pipe(
      map((user) => {
        if (!user) {
          this.toastr.error('You are not allowed');
          return false;
        }
        if (
          route.data.roles &&
          user.roles.some((r) => route.data.roles.includes(r))
        ) {
          return true;
        }
        this.toastr.error('You are not allowed');
        return false;
      })
    );
  }
}
