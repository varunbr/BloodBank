import { BaseModal } from './modal';
import { PageParams } from './pageParams';

export class AdminRole extends BaseModal {
  userName = '';
  photoUrl = '';
  name = '';
  role = 'Moderator';
}

export class AdminRoleParams extends PageParams {
  role = '';
  name = '';
  userName = '';
  userId = 0;
}
