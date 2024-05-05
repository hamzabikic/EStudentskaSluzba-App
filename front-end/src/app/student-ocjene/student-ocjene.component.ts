import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import {MojConfig} from "../MojConfig";
import {MyAuthService} from "../Services/MyAuthService";

@Component({
  selector: 'app-student-ocjene',
  templateUrl: './student-ocjene.component.html',
  styleUrls: ['./student-ocjene.component.css']
})
export class StudentOcjeneComponent implements OnInit {

  studentId =0;
  ime="";
  prezime="";
  ocjene:any;
  student:any;
  prosjek:any;
  dialog_noviPredmet = false;
  predmeti:any;
  predmetId:number=0;
  vrijednost="";
  upisnaOcjena ="";
  datumPolaganja="";
  poeni ="";
  ocjenaId =0;
  naslovDialog="Nova ocjena";

  constructor(private http: HttpClient, private route: ActivatedRoute, public auth: MyAuthService, private router: Router) { }

  async ngOnInit() {
    this.studentId = this.route.snapshot.params["id"];
    if(this.auth.isStudent()) {
      if(this.auth.getId() != this.studentId) {
        this.router.navigate([""]);
      }
    }
    console.log(this.studentId);
    await this.ucitajStudenta();
    await this.ucitajOcjene();
    await this.ucitajPredmete();
    this.izracunajProsjek();
  }
  async ucitajPredmete() {
    this.predmeti = await this.http.get(MojConfig.adresa_servera+"/Predmeti/getPredmeti").toPromise();
  }
  async ucitajStudenta() {
    this.student = await this.http.get(MojConfig.adresa_servera+"/Student/getById?id="+this.studentId).toPromise();
  }
  async ucitajOcjene () {
    this.ocjene = await this.http.get(MojConfig.adresa_servera+
      "/GetOcjeneEndpoint?id="+this.studentId).toPromise();
  }
  izracunajProsjek(){
    if((this.ocjene as []).length>0) {
      let suma = 0;
      let br_predmeta=0;
      for (let x of this.ocjene) {
           suma+=x.vrijednost;
           br_predmeta++;
      }
      this.prosjek = (suma/br_predmeta).toFixed(2).toString();
    }
  }
  async posaljiOcjenu() {
    if (this.ocjenaId == 0) {
      let obj = {
        predmetId: this.predmetId,
        studentId: this.studentId,
        vrijednost: this.vrijednost,
        upisnaOcjena: this.upisnaOcjena,
        datumPolaganja: this.datumPolaganja,
        poeni: this.poeni
      };
      let res = await this.http.post(MojConfig.adresa_servera + "/Ocjene/addOcjena", obj)
        .toPromise();
      //@ts-ignore
      if (res.dodana) {
        alert("Uspjesno dodana ocjena!");
        await this.ucitajOcjene();
        this.izracunajProsjek();
        this.dialog_noviPredmet = false;
        return;
      }
      //@ts-ignore
      alert(res.greska);
    }
    else {
      let obj = {
        vrijednost: this.vrijednost,
        upisnaOcjena: this.upisnaOcjena,
        datumPolaganja: this.datumPolaganja,
        poeni: this.poeni
      };
      let res = await this.http.put(MojConfig.adresa_servera+"/Ocjene/updateOcjena?id="+ this.ocjenaId,
        obj).toPromise();
      //@ts-ignore
      if (res.dodana) {
        alert("Uspjesno editovana ocjena!");
        await this.ucitajOcjene();
        this.izracunajProsjek();
        this.dialog_noviPredmet = false;
        return;
      }
      //@ts-ignore
      alert(res.greska);
    }
  }

  async obrisiOcjenu(id:any) {
    let res = await this.http.delete
    (MojConfig.adresa_servera+"/Ocjene/deleteOcjena?id="+id).toPromise();
    if(res) {
      alert("Uspjesno obrisana ocjena!");
      await this.ucitajOcjene();
      this.izracunajProsjek();
      return;
    }
    alert("Neuspjesno brisanje ocjene!");
  }
}
