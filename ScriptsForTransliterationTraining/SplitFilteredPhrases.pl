#!/usr/bin/perl
#==========File: NETrainAndEvaluate.pl==========
#Title:        NETrainAndEvaluate.pl - Named Entity Training and Evaluation.
#Description:  Trains a Stanford NER model on training directory data and evaluates the model on train and test directory data (separately).
#Author:       Mārcis Pinnis, SIA Tilde.
#Created:      May, 2011.
#Last Changes: 27.05.2011. by Mārcis Pinnis, SIA Tilde.
#===============================================

use strict;
use warnings;
use File::Basename;
use File::Copy;
use Encode;
use encoding "UTF-8";


BEGIN 
{
	use FindBin '$Bin'; #Gets the path of this file.
	push @INC, "$Bin";  #Adds the path of this file to places where Perl is searching for modules.
}
$Bin =~ s/\\/\//g;

my $inFile = $ARGV[0];
my $outFile = $ARGV[1];
my $srcLang = $ARGV[2];
my $trgLang = $ARGV[3];
my $totalLines = $ARGV[4];
my $devLines = $ARGV[5];

my $srcTrainFile = $outFile.".train.".$srcLang;
my $trgTrainFile = $outFile.".train.".$trgLang;
my $srcDevFile = $outFile.".dev.".$srcLang;
my $trgDevFile = $outFile.".dev.".$trgLang;

#Check if property templates exist and copy them to the working directory.
if (!(-e $inFile))
{
	print STDERR "ERROR: Missing input file $inFile.";
	die;
}
open(IN, "<:encoding(UTF-8)", $inFile);
open(OUT_SRC_TRAIN, ">:encoding(UTF-8)", $srcTrainFile);
open(OUT_TRG_TRAIN, ">:encoding(UTF-8)", $trgTrainFile);
open(OUT_SRC_DEV, ">:encoding(UTF-8)", $srcDevFile);
open(OUT_TRG_DEV, ">:encoding(UTF-8)", $trgDevFile);

my %idHash;
while (keys %idHash < $devLines)
{
	my $random_number = int(rand($totalLines-1));
	if (!defined($idHash{$random_number}))
	{
		$idHash{$random_number}=1;
	}
}

my $counter=0;
while (<IN>)
{
	my $line = $_;
	$line =~ s/^\x{FEFF}//; # cuts BOM
	$line =~ s/\n//;
	$line =~ s/\r//;
	my @lineData = split(/\t/,$line);
	my $len = @lineData;
	if ($len>=3)
	{
		my $src = $lineData[0];
		$src =~ s/^\s+//g;
		$src =~ s/\s+$//g;
		my @srcArr = split(//, $src);
		my $trg = $lineData[2];
		$trg =~ s/^\s+//g;
		$trg =~ s/\s+$//g;
		
		if ($src =~ /^-.*/ || $src =~ /.*-$/ || $trg =~ /^-.*/ || $trg =~ /.*-$/)
		{
			$counter++;
			next;
		}
		my @trgArr = split(//, $trg);
		
		if (defined($idHash{$counter}))
		{
			my $wasChar = 0;
			foreach my $i  (@srcArr)
			{
				#print $i." ".$i."\n";
				if ($wasChar == 0)
				{
					$wasChar = 1;
					print OUT_SRC_DEV $i;
				}
				else
				{
					print OUT_SRC_DEV " ".$i;
				}
			}
			$wasChar = 0;
			for my $i  (@trgArr)
			{
				if ($wasChar == 0)
				{
					$wasChar = 1;
					print OUT_TRG_DEV $i;
				}
				else
				{
					print OUT_TRG_DEV " ".$i;
				}
			}
			print OUT_SRC_DEV "\n";
			print OUT_TRG_DEV"\n";
		}
		else
		{
			my $wasChar = 0;
			foreach my $i  (@srcArr)
			{
				#print $i." ".$i."\n";
				if ($wasChar == 0)
				{
					$wasChar = 1;
					print OUT_SRC_TRAIN $i;
				}
				else
				{
					print OUT_SRC_TRAIN " ".$i;
				}
			}
			$wasChar = 0;
			for my $i  (@trgArr)
			{
				if ($wasChar == 0)
				{
					$wasChar = 1;
					print OUT_TRG_TRAIN $i;
				}
				else
				{
					print OUT_TRG_TRAIN " ".$i;
				}
			}
			print OUT_SRC_TRAIN "\n";
			print OUT_TRG_TRAIN"\n";
		}
	}
	$counter++;
}

close (IN);
close (OUT_SRC_TRAIN);
close (OUT_TRG_TRAIN);
close (OUT_SRC_DEV);
close (OUT_TRG_DEV);
exit;
