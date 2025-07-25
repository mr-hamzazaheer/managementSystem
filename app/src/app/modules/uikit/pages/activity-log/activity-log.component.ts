import { Component } from '@angular/core';
import { TableComponent } from 'src/app/shared/components/table/table.component';
import { UserService } from '../../services/user.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-activity-log',
  imports: [TableComponent],
  templateUrl: './activity-log.component.html',
  styleUrl: './activity-log.component.css'
})
export class ActivityLogComponent {
  constructor(private _userService:UserService) {}


  loadUsers = async (params: any) => {
  const jsonString = JSON.stringify(params);
  let res: any;
   res = await new Promise((resolve, reject) => { this._userService.GetAll({ filter: jsonString })
      .subscribe({
        next: (value) => resolve(value),
        error: (err) => reject(err),
        // No need for complete, as HTTP Observables complete after emission
      });
    });
  // const observable = await this._userService.GetAll({ filter: JSON.stringify({ filter: jsonString }) });
  //   res = await firstValueFrom(observable);
  const { httpCode, data, message } = res || {};

  return {
    data: data,
    total: data?.length || 0,
  };
};

}
