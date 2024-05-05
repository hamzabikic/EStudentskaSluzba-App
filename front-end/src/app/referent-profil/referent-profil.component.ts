import { Component, OnInit } from '@angular/core';
import {MyAuthService} from "../Services/MyAuthService";
import {MojConfig} from "../MojConfig";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-referent-profil',
  templateUrl: './referent-profil.component.html',
  styleUrls: ['./referent-profil.component.css']
})
export class ReferentProfilComponent implements OnInit {
  id:any;
  slika ="";
  ucitana_slika = false;
  referent:any;
  datumRodjenja ="";
  opstine:any;
  staraLozinka ="";
  novaLozinka ="";
  constructor(private auth: MyAuthService, private http: HttpClient) {
    this.id = auth.getId();
  }

  async ngOnInit() {
    this.slika = MojConfig.adresa_servera +"/Image/getImageById?id="+ this.id;
    await this.ucitajOpstine();
    await this.ucitajReferenta();

  }
  async posaljiSliku() {
    let obj = {
      studentid: this.id,
      base64string:this.slika
    };
    let res = await this.http.post(MojConfig.adresa_servera+"/Image/dodajSliku",obj).toPromise();
    if(res) {
      alert("Uspjesno ucitana slika!");
      this.ucitana_slika = false;
      this.slika = MojConfig.adresa_servera +"/Image/getImageById?id="+ this.id;
      return;
    }
    alert("Neuspjesno ucitana slika!");
  }
  async ucitajOpstine() {
    this.opstine = await this.http.get(MojConfig.adresa_servera + "/Opstina/getOpstine").toPromise();
  }
  async ucitajReferenta () {
    this.referent = await this.http.get(MojConfig.adresa_servera +"/Korisnik/getReferentById?id=" + this.id).toPromise();
    this.datumRodjenja = this.referent.datumRodjenja.split('T')[0];
  }
  async editujReferenta() {
    let obj = {
         referentId:this.id,
         ime: this.referent.ime,
         prezime: this.referent.prezime,
         datumRodjenja : this.datumRodjenja,
         opstinaId : this.referent.opstinaId,
         korisnickoIme: this.referent.korisnickoIme,
         email: this.referent.email
    };
    let res = await this.http.put(MojConfig.adresa_servera +"/Korisnik/referentEdit", obj).toPromise();
    //@ts-ignore
    if(res.editovan) {
      if(this.ucitana_slika) {
        await this.posaljiSliku();
      }
      alert("Uspjesno editovanje!");
      await this.ucitajReferenta();
      return;
    }
    //@ts-ignore
    alert(res.greska);
  }
  async promjenaLozinke () {
     let obj = {
       studentId: this.id,
       staraLozinka : this.staraLozinka,
       novaLozinka: this.novaLozinka
     };
     let res = await this.http.patch(MojConfig.adresa_servera+"/Korisnik/NovaLozinka", obj).toPromise();
     // @ts-ignore
    if(res.editovan) {
       alert("Uspjesno izmijenjena lozinka!");
       this.staraLozinka="";
       this.novaLozinka="";
       return;
     }
     // @ts-ignore
    alert(res.greska);
  }
  ucitajSliku() {
    //@ts-ignore
    let file = document.getElementById("slika-file").files[0];
    if(file) {
      let reader = new FileReader();
      reader.onload = () => {
        // @ts-ignore
        this.slika = reader.result!.toString();
        this.ucitana_slika = true;
      };
      reader.readAsDataURL(file);
      return;
    }
      this.slika="";
      this.ucitana_slika = false;

  }

  protected readonly MojConfig = MojConfig;
}
