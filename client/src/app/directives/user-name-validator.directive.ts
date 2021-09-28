import { Directive, Input } from '@angular/core';
import {
  AbstractControl,
  AsyncValidator,
  AsyncValidatorFn,
  NG_ASYNC_VALIDATORS,
  ValidationErrors,
} from '@angular/forms';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appUserNameValidator]',
  providers: [
    {
      provide: NG_ASYNC_VALIDATORS,
      useExisting: UserNameValidatorDirective,
      multi: true,
    },
  ],
})
export class UserNameValidatorDirective implements AsyncValidator {
  @Input('appUserNameValidator') exist;
  constructor(private accountService: AccountService) {}
  validate(
    control: AbstractControl
  ): Promise<ValidationErrors> | Observable<ValidationErrors> {
    return validateUserExistence(this.accountService, this.exist)(control);
  }
}
export function validateUserExistence(
  accountService: AccountService,
  exist = true
): AsyncValidatorFn {
  return (control: AbstractControl) => {
    if (control?.value && control.dirty) {
      return accountService.userExist(control.value).pipe(
        map((response) => {
          if (response.toString() === exist.toString()) return null;
          return { alreadyExist: response };
        })
      );
    }
    return of(null);
  };
}
