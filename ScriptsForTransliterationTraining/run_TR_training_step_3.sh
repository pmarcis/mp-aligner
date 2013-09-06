#Train the translation model.
#cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/LT-EN/
#/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/lt_en_dgt.train -f lt -e en -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/en_mono.blm.en:8 >& training.out &

#cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/ET-EN/
#/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/et_en_dgt.train -f et -e en -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/en_mono.blm.en:8 >& training.out &

#cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/SL-EN/
#/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/sl_en_dgt.train -f sl -e en -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/en_mono.blm.en:8 >& training.out &

#cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/EL-EN/
#/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/el_en_dgt.train -f el -e en -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/en_mono.blm.en:8 >& training.out &

#cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/RO-EN/
#/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/ro_en_dgt.train -f ro -e en -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/en_mono.blm.en:8 >& training.out &

#cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/DE-EN/
#/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/de_en_dgt.train -f de -e en -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/en_mono.blm.en:8 >& training.out &

#cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/HR-EN/
#/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/hr_en_dgt.train -f hr -e en -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/en_mono.blm.en:8 >& training.out &

#cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/LV-LT/
#/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/lv_lt_dgt.train -f lv -e lt -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.lt:8 >& training.out &

#cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/LT-LV/
#/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/lt_lv_dgt.train -f lt -e lv -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.lv:8 >& training.out &

cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/EN-LV/
/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/lv-en-mt4.train -f en -e lv -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.lv:8 >& training.out &

cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/EN-LT/
/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/lt_en_dgt.train -f en -e lt -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.lt:8 >& training.out &

cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/EN-ET/
/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/et_en_dgt.train -f en -e et -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.et:8 >& training.out &

cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/EN-SL/
/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/sl_en_dgt.train -f en -e sl -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.sl:8 >& training.out &

cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/EN-EL/
/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/el_en_dgt.train -f en -e el -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.el:8 >& training.out &

cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/EN-RO/
/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/ro_en_dgt.train -f en -e ro -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.ro:8 >& training.out &

cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/EN-DE/
/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/de_en_dgt.train -f en -e de -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.de:8 >& training.out &

cd /home/marcis/TILDE/RESOURCES/TRANSLIT_WORKING_DIR/EN-HR/
/home/marcis/LU/moses/mosesdecoder/scripts/training/train-model.perl -external-bin-dir /home/marcis/LU/moses/external-bin-dir -root-dir train -corpus /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/hr_en_dgt.train -f en -e hr -alignment grow-diag-final-and -reordering msd-bidirectional-fe -lm 0:5:/home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.blm.hr:8 >& training.out &
