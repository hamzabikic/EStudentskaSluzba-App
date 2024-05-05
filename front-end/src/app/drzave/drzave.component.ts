import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-drzave',
  templateUrl: './drzave.component.html',
  styleUrls: ['./drzave.component.css']
})
export class DrzaveComponent implements OnInit {

  naziv="";
  dialog_nova_drzava = false;
  drzave:any;
  drzave_copy:any;
  ime ="";
  constructor(private http:HttpClient) { }

  async ngOnInit(){
    await this.getDrzave();
  }
  pretraga() {
    this.drzave_copy = this.drzave.
    filter((p:any)=> p.naziv.toLowerCase().includes(this.naziv.toLowerCase()));
  }
  async getDrzave() {
    this.drzave= await this.http.get(MojConfig.adresa_servera + "/Opstina/getDrzave").toPromise();
    this.drzave_copy = this.drzave;
  }
  async dodajDrzavu() {
    let obj = {
      naziv: this.ime
    };
    let res = await this.http.post(MojConfig.adresa_servera+"/Opstina/addDrzava", obj).toPromise();
    if(res) {
      alert("Uspjesno dodana drzava!");
      await this.getDrzave();
      await this.pretraga();
      this.obrisiPodatke();
      this.dialog_nova_drzava = !this.dialog_nova_drzava;
      return;
    }
    alert("Neuspjesno dodavanje drzave!");
  }
  obrisiPodatke() {
    this.ime ="";
  }
  async obrisiDrzavu(id:number) {
    let res = await this.http.delete
    (MojConfig.adresa_servera + "/Opstina/deleteDrzava?drzavaid="+id).toPromise();
    if(res) {
      await this.getDrzave();
      await this.pretraga();
      alert("Uspjesno brisanje drzave!");
      return;
    }
    alert("Neuspjesno brisanje drzave!");
  }

}
