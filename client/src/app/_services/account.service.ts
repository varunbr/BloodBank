import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Role } from '../_modals/admin';
import { UserProfile } from '../_modals/profile';
import { User } from '../_modals/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private userSource = new ReplaySubject<User>(1);
  user$ = this.userSource.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  login(model: any) {
    return this.http
      .post<User>(environment.apiUrl + 'account/login', model)
      .pipe(
        map((user: User) => {
          if (user) {
            this.setUser(user);
          }
        })
      );
  }

  setUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? (user.roles = roles) : (user.roles = [roles]);
    localStorage.setItem('user', JSON.stringify(user));
    this.userSource.next(null);
    this.userSource.next(user);
  }

  register(model: any) {
    return this.http
      .post<User>(environment.apiUrl + 'account/register', model)
      .pipe(
        map((user: User) => {
          if (user) {
            this.setUser(user);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.userSource.next();
    let extras: NavigationExtras = { state: { reload: true } };
    this.router.navigateByUrl('/login', extras);
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }

  userExist(userName: string) {
    return this.http.get(environment.apiUrl + 'account/' + userName, {
      headers: { BackgroundLoad: 'true' },
    });
  }

  getProfile() {
    return this.http.get<UserProfile>(environment.apiUrl + 'account/profile');
  }

  updateProfile(body) {
    return this.http
      .post<UserProfile>(environment.apiUrl + 'account/profile', body)
      .pipe(
        map((response) => {
          this.updateToken();
          return response;
        })
      );
  }

  updateToken() {
    this.http
      .post<User>(environment.apiUrl + 'account/token-update', {})
      .subscribe((user) => {
        if (user) {
          this.setUser(user);
        }
      });
  }

  changePhoto(fileToUpload: File) {
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload);
    return this.http
      .post<any>(environment.apiUrl + 'account/change-photo', formData)
      .pipe(
        map((response) => {
          this.updateToken();
          return response;
        })
      );
  }

  removePhoto() {
    const formData: FormData = new FormData();
    formData.append('remove', 'true');
    return this.http
      .post<any>(environment.apiUrl + 'account/change-photo', formData)
      .pipe(
        map((response) => {
          this.updateToken();
          return response;
        })
      );
  }

  getRolesForAbout() {
    return this.http.get<Role[]>(environment.apiUrl + 'account/about');
  }
}
