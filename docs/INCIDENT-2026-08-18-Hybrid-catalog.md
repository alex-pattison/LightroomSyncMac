# Incident: Hybrid.lrcat lost develop-settings rows

**When:** 2026-08-04 22:27–22:53 (Pine, device-switch zip)  
**Catalog:** `Hybrid.lrcat` (Adobe Lightroom Classic, synced identity preserved)  
**Status:** Restored 2026-08-18 by copying `Adobe_imageDevelopSettings` from the last healthy zip.

This is the failure mode Lightroom Sync+ must not repeat. Full diagnostic workspace: `H:\Photography\Claude\LightroomCatalogIssue260818` (local).

## What broke

Lightroom would not build standard previews in some folders (~1 image every few seconds, no CPU/disk/GPU). Other folders were instant. Files were fine (fresh catalog + same photos = normal speed). SQLite `integrity_check` was **ok**.

**~69,927 images had no row in `Adobe_imageDevelopSettings`.** Previews render from that table. Missing row → Lightroom falls into a slow blocked recovery path per image.

Edits were not gone from the catalog: `Adobe_libraryImageDevelopHistoryStep.text` still held zlib-compressed settings. XMP was off, so **Metadata > Read Metadata from Files** is the wrong repair (it writes camera defaults and cements the loss). That was confirmed on folder `2025/A7IV/2025-10-05`.

## Timeline (zips this app writes)

| Zip | Time | images | develop rows | missing_dev |
|---|---|---|---|---|
| Pine `F:\Pine_Lightroom_BU\2026-08-04 2223` | 22:23 | 75,570 | 75,570 | **0** |
| Cloud `Hybrid - 2026-08-04 22-27.zip` | 22:27 | 75,570 | 75,570 | **0** |
| Cloud `Hybrid - 2026-08-04 22-53.zip` | 22:53 | 75,570 | 5,591 | **69,979** |

Same image count. Develop table emptied. 22:53 zip was ~340 MB smaller. Later zips (Aug 11, 16, 18) stayed damaged until a manual SQL restore.

That 26-minute gap is a **second device-switch zip** while the catalog was already (or became) gutted — Lightroom open, WAL not checkpointed, or this app copying `Hybrid.lrcat` mid-write.

## Restore that worked (2026-08-18)

Do **not** adopt the old backup as the live catalog (would drop later imports). Do **not** Export as Catalog (new catalog identity; Adobe cloud sync breaks).

1. Quit Lightroom.
2. If Read Metadata was run on any folder, those images now have **default** develop rows. A hole-fill restore will skip them. Swap back a copy from **before** Read Metadata first (TEST copy still had holes on Oct 2025).
3. Detector must read the damage count (was 69,927).
4. `INSERT` develop rows from the newest zip with `missing_dev = 0`, **only where the current catalog has no row** for that `image` id. Do not overwrite post-backup imports (e.g. 2026-08-06 Q2M onward).
5. Confirm detector **0**, `integrity_check` ok, then open Lightroom with **sync paused**.

All 69,927 holes existed in the 22:23/22:27 backups (`missing_not_in_backup = 0`).

Detector:

```sql
SELECT COUNT(*) FROM Adobe_images i
LEFT JOIN Adobe_imageDevelopSettings ds ON ds.image = i.id_local
WHERE ds.image IS NULL;
```

Healthy is **0**.

## If this happens again

1. Quit Lightroom. Do not Read Metadata from Files.
2. Run the detector on the live `.lrcat` (sqlite3 `-readonly`).
3. Extract **only** `Hybrid.lrcat` from backups, newest first, until `missing_dev` is 0. Keep that zip.
4. On a **copy**, SQL-copy `Adobe_imageDevelopSettings` from the healthy backup where `NOT EXISTS` a current row for `ds.image`. Omit `id_local` so SQLite assigns new keys.
5. Point `Adobe_images.developSettingsIDCache` at the new row ids.
6. Verify detector = 0, then open the copy in Lightroom (pause Adobe sync). If loupe + hanging folders look right, apply the same SQL to the live catalog while Lightroom is still closed.
7. Keep `Hybrid.lrcat-data` paired with whichever `.lrcat` you keep.

A one-shot script is enough; a separate GUI app is not. The durable fix is **this program refusing to upload a catalog that is open or already missing develop rows**, and refusing to replace a local catalog with a zip whose `missing_dev` jumped.

## Hardening for Lightroom Sync+ (not built yet)

- Refuse zip/upload if `Lightroom.exe` is running.
- After zip, run the detector on the archived `.lrcat` (or on a `VACUUM INTO` snapshot). Abort upload if `missing_dev` is above a small threshold **and** worse than the last successful upload.
- On download/replace: same check before overwriting the local catalog. Keep the existing “backup on replace” copy.
- Log image count, develop count, missing_dev next to each zip.

## Related paths (Pine)

- Live catalog: `H:\Photography\Lightroom_Hybrid_Catalog\Hybrid.lrcat`
- Device-switch zips: `Z:\My Drive\Photography\Lightroom_Hybrid_Catalog_Sync\`
- Last known good from this incident: `F:\Pine_Lightroom_BU\2026-08-04 2223`
- Adobe mobile originals (flat, no date subfolders): `Z:\My Drive\Photography\Google_Mobile_Sync`
