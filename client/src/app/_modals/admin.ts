import { BaseModal } from './modal';
import { PageParams } from './pageParams';

export class Role extends BaseModal {
  userId = 0;
  userName = '';
  photoUrl = '';
  name = '';
  role = '';
  constructor(role: string) {
    super();
    this.role = role;
  }
}

export class RoleParams extends PageParams {
  role = '';
  name = '';
  userName = '';
  userId = 0;
}
