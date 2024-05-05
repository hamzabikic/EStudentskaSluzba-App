import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import {FormsModule} from "@angular/forms";
import {RouterModule, RouterOutlet} from "@angular/router";
import { StudentiComponent } from './studenti/studenti.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import { StudentEditComponent } from './student-edit/student-edit.component';
import { LoginComponent } from './login/login.component';
import {MyAuthService} from "./Services/MyAuthService";
import {MyHttpInterceptor} from "./Services/MyHttpInterceptor";
import {LoginCanActivate} from "./Services/LoginCanActivate";
import {AkcijeCanActivate} from "./Services/AkcijeCanActivate";
import { OcjeneComponent } from './ocjene/ocjene.component';
import { StudentOcjeneComponent } from './student-ocjene/student-ocjene.component';
import {OcjeneCanActivate} from "./Services/OcjeneCanActivate";
import { UplateComponent } from './uplate/uplate.component';
import { StudentUplateComponent } from './student-uplate/student-uplate.component';
import { RateComponent } from './rate/rate.component';
import { ProfesoriComponent } from './profesori/profesori.component';
import { ProfesorEditComponent } from './profesor-edit/profesor-edit.component';
import { PredmetiComponent } from './predmeti/predmeti.component';
import { OpstineComponent } from './opstine/opstine.component';
import { DrzaveComponent } from './drzave/drzave.component';
import { SmjeroviComponent } from './smjerovi/smjerovi.component';
import { ReferentProfilComponent } from './referent-profil/referent-profil.component';
import { StudentProfilComponent } from './student-profil/student-profil.component';
import { NastavnikProfilComponent } from './nastavnik-profil/nastavnik-profil.component';
import {StudentProfilCanActivate} from "./Services/StudentProfilCanActivate";
import {ProfesorProfilCanActivate} from "./Services/ProfesorProfilCanActivate";
import {ReferentProfilCanActivate} from "./Services/ReferentProfilCanActivate";

@NgModule({
  declarations: [
    AppComponent,
    StudentiComponent,
    StudentEditComponent,
    LoginComponent,
    OcjeneComponent,
    StudentOcjeneComponent,
    UplateComponent,
    StudentUplateComponent,
    RateComponent,
    ProfesoriComponent,
    ProfesorEditComponent,
    PredmetiComponent,
    OpstineComponent,
    DrzaveComponent,
    SmjeroviComponent,
    ReferentProfilComponent,
    StudentProfilComponent,
    NastavnikProfilComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RouterOutlet,
    RouterModule.forRoot([
      {path:"studenti", component:StudentiComponent , canActivate:[AkcijeCanActivate]},
      {path:"studenti/edit/:id", component:StudentEditComponent, canActivate:[AkcijeCanActivate]},
      {path:"studenti/detalji/:id", component: StudentEditComponent, canActivate:[AkcijeCanActivate]},
      {path:"login", component:LoginComponent, canActivate:[LoginCanActivate]},
      {path:"ocjene", component:OcjeneComponent, canActivate:[OcjeneCanActivate]},
      {path:"student/ocjene/:id", component:StudentOcjeneComponent},
      {path:"uplate", component:UplateComponent, canActivate:[AkcijeCanActivate]},
      {path:"student/uplate/:id", component:StudentUplateComponent, canActivate:[AkcijeCanActivate]},
      {path:"student/rate/:id", component:RateComponent, canActivate:[AkcijeCanActivate]},
      {path:"profesori", component:ProfesoriComponent, canActivate:[AkcijeCanActivate]},
      {path:"profesori/edit/:id", component:ProfesorEditComponent, canActivate:[AkcijeCanActivate]},
      {path:"profesori/detalji/:id", component:ProfesorEditComponent, canActivate:[AkcijeCanActivate]},
      {path:"predmeti", component:PredmetiComponent, canActivate:[AkcijeCanActivate]},
      {path:"opstine", component:OpstineComponent, canActivate:[AkcijeCanActivate]},
      {path:"drzave", component:DrzaveComponent, canActivate:[AkcijeCanActivate]},
      {path:"smjerovi", component:SmjeroviComponent, canActivate:[AkcijeCanActivate]},
      {path:"", component:LoginComponent,canActivate:[LoginCanActivate]},
      {path:"student/profil", component:StudentProfilComponent,canActivate:[StudentProfilCanActivate]},
      {path:"profesor/profil", component:NastavnikProfilComponent, canActivate:[ProfesorProfilCanActivate]},
      {path:"referent/profil", component:ReferentProfilComponent, canActivate:[ReferentProfilCanActivate]},
      {path:"**", redirectTo:""}
    ]),
    HttpClientModule
  ],
  providers: [MyAuthService,
    {provide:HTTP_INTERCEPTORS, multi:true, useClass:MyHttpInterceptor },
  LoginCanActivate, AkcijeCanActivate, OcjeneCanActivate, StudentProfilCanActivate, ProfesorProfilCanActivate,
  ReferentProfilCanActivate],
  bootstrap: [AppComponent],

})
export class AppModule { }
