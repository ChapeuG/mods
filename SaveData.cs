using System;
using System.Collections.Generic;

// Token: 0x02000054 RID: 84
[Serializable]
public class SaveData
{
	// Token: 0x04000553 RID: 1363
	public string martName_SD;

	// Token: 0x04000554 RID: 1364
	public string saveDate_SD;

	// Token: 0x04000555 RID: 1365
	public float playTime_SD;

	// Token: 0x04000556 RID: 1366
	public float money_SD;

	// Token: 0x04000557 RID: 1367
	public int awards_SD;

	// Token: 0x04000558 RID: 1368
	public bool martOpen_SD;

	// Token: 0x04000559 RID: 1369
	public float playerPosX_SD;

	// Token: 0x0400055A RID: 1370
	public float playerPosY_SD;

	// Token: 0x0400055B RID: 1371
	public float playerPosZ_SD;

	// Token: 0x0400055C RID: 1372
	public int playerSkinMatIndex_SD;

	// Token: 0x0400055D RID: 1373
	public int playerEyesMatIndex_SD;

	// Token: 0x0400055E RID: 1374
	public int playerClothesMatIndex_SD;

	// Token: 0x0400055F RID: 1375
	public int playerHairMatIndex_SD;

	// Token: 0x04000560 RID: 1376
	public int playerHairMeshIndex_SD;

	// Token: 0x04000561 RID: 1377
	public int playerHatMatIndex_SD;

	// Token: 0x04000562 RID: 1378
	public int playerHatGoIndex_SD;

	// Token: 0x04000563 RID: 1379
	public int[] worldTime_SD = new int[2];

	// Token: 0x04000564 RID: 1380
	public int[] prodSellPrice_SD;

	// Token: 0x04000565 RID: 1381
	public List<int> inv_AffectedProds_SD = new List<int>();

	// Token: 0x04000566 RID: 1382
	public List<int> inv_Deals_Indexes = new List<int>();

	// Token: 0x04000567 RID: 1383
	public List<int> inv_Deals_DaysLeft = new List<int>();

	// Token: 0x04000568 RID: 1384
	public int currentLevelIndex_SD;

	// Token: 0x04000569 RID: 1385
	public int currentExpansionIndex_SD;

	// Token: 0x0400056A RID: 1386
	public int currentExpansionRemainingDays_SD;

	// Token: 0x0400056B RID: 1387
	public int currentSeasonIndex_SD;

	// Token: 0x0400056C RID: 1388
	public List<int> shelfProdIndex_SD = new List<int>();

	// Token: 0x0400056D RID: 1389
	public List<SerializableVector3> shelfProdPosition_SD = new List<SerializableVector3>();

	// Token: 0x0400056E RID: 1390
	public List<SerializableVector3> shelfProdRotation_SD = new List<SerializableVector3>();

	// Token: 0x0400056F RID: 1391
	public List<SerializableVector3> shelfProdProductsIndex_SD = new List<SerializableVector3>();

	// Token: 0x04000570 RID: 1392
	public List<SerializableVector3> shelfProdProductsQnt_SD = new List<SerializableVector3>();

	// Token: 0x04000571 RID: 1393
	public List<bool> shelfProdBrokenState_SD = new List<bool>();

	// Token: 0x04000572 RID: 1394
	public List<SerializableVector3> shelfInvBoxType_SD = new List<SerializableVector3>();

	// Token: 0x04000573 RID: 1395
	public List<SerializableVector3> shelfInvBoxIndex_SD = new List<SerializableVector3>();

	// Token: 0x04000574 RID: 1396
	public List<SerializableVector3> shelfInvBoxQnt_SD = new List<SerializableVector3>();

	// Token: 0x04000575 RID: 1397
	public List<SerializableVector3> shelfInvBoxLifeSpanIndex_SD = new List<SerializableVector3>();

	// Token: 0x04000576 RID: 1398
	public List<SerializableVector3> shelfInvBoxFrozen_SD = new List<SerializableVector3>();

	// Token: 0x04000577 RID: 1399
	public List<int> decorIndex_SD = new List<int>();

	// Token: 0x04000578 RID: 1400
	public List<SerializableVector3> decorPosition_SD = new List<SerializableVector3>();

	// Token: 0x04000579 RID: 1401
	public List<SerializableVector3> decorRotation_SD = new List<SerializableVector3>();

	// Token: 0x0400057A RID: 1402
	public List<int> decorLifeSpanIndex_SD = new List<int>();

	// Token: 0x0400057B RID: 1403
	public List<int> wallIndex_SD = new List<int>();

	// Token: 0x0400057C RID: 1404
	public List<int> floorIndex_SD = new List<int>();

	// Token: 0x0400057D RID: 1405
	public List<int> utilIndex_SD = new List<int>();

	// Token: 0x0400057E RID: 1406
	public List<SerializableVector3> utilPosition_SD = new List<SerializableVector3>();

	// Token: 0x0400057F RID: 1407
	public List<SerializableVector3> utilRotation_SD = new List<SerializableVector3>();

	// Token: 0x04000580 RID: 1408
	public List<SerializableVector3> utilBoxType_SD = new List<SerializableVector3>();

	// Token: 0x04000581 RID: 1409
	public List<SerializableVector3> utilBoxIndex_SD = new List<SerializableVector3>();

	// Token: 0x04000582 RID: 1410
	public List<SerializableVector3> utilBoxQnt_SD = new List<SerializableVector3>();

	// Token: 0x04000583 RID: 1411
	public List<SerializableVector3> utilLifeSpanIndex_SD = new List<SerializableVector3>();

	// Token: 0x04000584 RID: 1412
	public List<bool> utilBrokenState_SD = new List<bool>();

	// Token: 0x04000585 RID: 1413
	public List<int> boxIndex_SD = new List<int>();

	// Token: 0x04000586 RID: 1414
	public List<int> boxType_SD = new List<int>();

	// Token: 0x04000587 RID: 1415
	public List<int> boxQnt_SD = new List<int>();

	// Token: 0x04000588 RID: 1416
	public List<int> boxLifeSpanIndex_SD = new List<int>();

	// Token: 0x04000589 RID: 1417
	public List<bool> boxFrozen_SD = new List<bool>();

	// Token: 0x0400058A RID: 1418
	public List<SerializableVector3> boxPosition_SD = new List<SerializableVector3>();

	// Token: 0x0400058B RID: 1419
	public List<SerializableVector3> boxRotation_SD = new List<SerializableVector3>();

	// Token: 0x0400058C RID: 1420
	public bool[,] item_Unlocked_SD = new bool[99, 999];

	// Token: 0x0400058D RID: 1421
	public bool[,] item_NewlyUnlocked_SD = new bool[99, 999];

	// Token: 0x0400058E RID: 1422
	public List<int> deliveryIndexes_SD = new List<int>();

	// Token: 0x0400058F RID: 1423
	public List<int> deliveryCategories_SD = new List<int>();

	// Token: 0x04000590 RID: 1424
	public List<int> deliveryQnt_SD = new List<int>();

	// Token: 0x04000591 RID: 1425
	public List<int> deliverySupplierIndexes_SD = new List<int>();

	// Token: 0x04000592 RID: 1426
	public List<int> deliveryDaysIndexes_SD = new List<int>();

	// Token: 0x04000593 RID: 1427
	public List<int> deliveryLifeSpanIndexes_SD = new List<int>();

	// Token: 0x04000594 RID: 1428
	public float createCustomerTimer_SD;

	// Token: 0x04000595 RID: 1429
	public List<int> customerIndex_SD = new List<int>();

	// Token: 0x04000596 RID: 1430
	public List<SerializableVector3> customerPosition_SD = new List<SerializableVector3>();

	// Token: 0x04000597 RID: 1431
	public List<string> customerProdList_SD = new List<string>();

	// Token: 0x04000598 RID: 1432
	public List<bool> customerDoneShopping = new List<bool>();

	// Token: 0x04000599 RID: 1433
	public List<bool> customerDoneCashier = new List<bool>();

	// Token: 0x0400059A RID: 1434
	public List<float> customerRatesByIndex = new List<float>();

	// Token: 0x0400059B RID: 1435
	public List<bool> customerLifeAchievementsByIndex = new List<bool>();

	// Token: 0x0400059C RID: 1436
	public List<int> customerProdWantedNowByIndex = new List<int>();

	// Token: 0x0400059D RID: 1437
	public List<LocalCustomer_Data> localCustomer_Datas = new List<LocalCustomer_Data>();

	// Token: 0x0400059E RID: 1438
	public float taigaScore;

	// Token: 0x0400059F RID: 1439
	public List<int> taigaAwards_AwardsWonQnt = new List<int>();

	// Token: 0x040005A0 RID: 1440
	public List<float> finances_OutProd = new List<float>();

	// Token: 0x040005A1 RID: 1441
	public List<float> finances_OutFurniture = new List<float>();

	// Token: 0x040005A2 RID: 1442
	public List<float> finances_OutStaff = new List<float>();

	// Token: 0x040005A3 RID: 1443
	public List<float> finances_OutExpansion = new List<float>();

	// Token: 0x040005A4 RID: 1444
	public List<float> finances_OutMarketing = new List<float>();

	// Token: 0x040005A5 RID: 1445
	public List<float> finances_OutOperational = new List<float>();

	// Token: 0x040005A6 RID: 1446
	public List<float> finances_InSales = new List<float>();

	// Token: 0x040005A7 RID: 1447
	public List<float> finances_InPrizes = new List<float>();

	// Token: 0x040005A8 RID: 1448
	public int world_SeasonIndex;

	// Token: 0x040005A9 RID: 1449
	public int world_DayOverall;

	// Token: 0x040005AA RID: 1450
	public int world_DayIndex;

	// Token: 0x040005AB RID: 1451
	public int world_WeekIndex;

	// Token: 0x040005AC RID: 1452
	public List<int> world_Forecast;

	// Token: 0x040005AD RID: 1453
	public List<Staff_Data> staff_Data = new List<Staff_Data>();

	// Token: 0x040005AE RID: 1454
	public List<Staff_SaveData> staff_SaveData = new List<Staff_SaveData>();

	// Token: 0x040005AF RID: 1455
	public List<Staff_Data> staff_Possible_Data = new List<Staff_Data>();

	// Token: 0x040005B0 RID: 1456
	public List<List<TaskSaveData>> taskSaveData_SD = new List<List<TaskSaveData>>();

	// Token: 0x040005B1 RID: 1457
	public List<Mail_Data> mail_data_SD = new List<Mail_Data>();

	// Token: 0x040005B2 RID: 1458
	public EasterEgg_Data easterEgg_data_SD = new EasterEgg_Data();
}
