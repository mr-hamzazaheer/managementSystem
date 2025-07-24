import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UikitComponent } from './uikit.component';
import { TableComponent } from './pages/table/table.component';
import { UserComponent } from './pages/user/user.component';
import { ActivityLogComponent } from './pages/activity-log/activity-log.component';

const routes: Routes = [
  {
    path: '',
    component: UikitComponent,
    children: [
      { path: '', redirectTo: 'System Management', pathMatch: 'full' },
      { path: 'role', component: TableComponent },
      { path: 'user', component: UserComponent },
      { path: 'activity-log', component: ActivityLogComponent },
      { path: '**', redirectTo: 'errors/404' },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UikitRoutingModule {}
